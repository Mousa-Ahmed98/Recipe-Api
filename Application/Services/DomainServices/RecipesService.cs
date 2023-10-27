using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;

using Core.Entities;
using Core.Enums;
using Core.Common;
using Core.Interfaces;
using Core.CustomModels;

using Infrastructure.Exceptions.Recipe;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.User;

using Application.Interfaces;
using Application.Interfaces.DomainServcies;
using Application.DTOs.Request;
using Application.DTOs.Response;

namespace Application.Services.DomainServices
{
    public class RecipesService : IRecipesService
    {
        private readonly IMapper _mapper;
        private readonly IUserSession _session;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationManager _notificationManager;

        public RecipesService(
            IMapper mapper,
            IUserSession session,
            IImageService imageService,
            IUnitOfWork unitOfWork,
            INotificationManager notificationManager
            )
        {
            _mapper = mapper;
            _session = session;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
            _notificationManager = notificationManager;
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesSummary(GetRecipeRequest request)
        {
           return await _unitOfWork.RecipeRepository.GetRecipesSummary(
                _session.UserId,
               request.PageNumber, 
               request.PageSize, 
               request.Category
               );
        }

        public async Task<RecipeResponse> GetOneById(int id)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetOneById(id, _session.UserId);
            
            if(recipe == null)
            {
                throw new RecipeNotFoundException(id);
            }

            return _mapper.Map<RecipeResponse>(recipe);
        }

        public async Task<PaginatedList<RecipeSummary>> FilterByIngredients
            (FilteredRecipeRequest request)
        {
            List<string> filterIngredients = request.Ingredients.Split(',').ToList();

            var res = await _unitOfWork.RecipeRepository.FilterByIngredients(
                _session.UserId, request.PageNumber, request.PageSize, filterIngredients
                );

            return res;
        }

        public Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, PaginatedRequest request)
        {
            return _unitOfWork.RecipeRepository.SearchRecipes(
                    query, request.PageNumber, request.PageSize
                    );
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesByUsername
            (string username, PaginatedRequest request)
        {
            var author = await _unitOfWork.UsersRepository.GetByUsername(_session.UserId, username);

            if (author == null)
            {
                throw new UserNotFoundException(username);
            }

            return await _unitOfWork.RecipeRepository.GetRecipesByAuthorId(
                    _session.UserId, author.Id, request.PageNumber, request.PageSize
                    );
        }

        public async Task<PaginatedList<RecipeSummary>> GetMyRecipesAsync(PaginatedRequest request)
        {
           return await _unitOfWork.RecipeRepository.GetRecipesByAuthorId(
               _session.UserId, _session.UserId, request.PageNumber, request.PageSize
               );
        }

        public async Task<RecipeResponse> CreateNewRecipe(RecipeRequest recipeRequest)
        {
            var newRecipe = _mapper.Map<Recipe>(recipeRequest);

            if (!string.IsNullOrEmpty(recipeRequest.ImageData))
            {
                var imageName = await _imageService
                    .SaveImageAsync(recipeRequest.ImageData, recipeRequest.Name);

                newRecipe.ImageName = imageName;
            }

            newRecipe.AuthorId = _session.UserId;

            await _unitOfWork.RecipeRepository.AddAsync(newRecipe);
            await _unitOfWork.SaveAsync();

            // notify followers
            var followers = await _unitOfWork.FollowsRepoisitory.GetFollowersByUserId(newRecipe.AuthorId);
            
            // but we won't wait for this to complete. recipe now exists & it can complete on its own.
            _ = _notificationManager.CreateMany(
                usersIds: followers.Select(x => x.Id).ToList(), 
                recipe: _mapper.Map<RecipeSummary>(newRecipe),
                type: NotificationType.NewPost
                );
            
            return _mapper.Map<RecipeResponse>(newRecipe);
        }

        public async Task<RecipeResponse> UpdateRecipe(int recipeId, RecipeRequest recipeRequest)
        {
            var existingRecipe = await _unitOfWork.RecipeRepository
                .GetOneById(recipeId, _session.UserId);

            if (existingRecipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            if (existingRecipe.AuthorId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            var category = await _unitOfWork.CategoriesRepository
                .GetByIdAsync(recipeRequest.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category Not found");
            }

            if (!string.IsNullOrEmpty(recipeRequest.ImageData))
            {
                // delete old image 
                if(!string.IsNullOrEmpty(existingRecipe.ImageName))
                {
                    _imageService.RemoveImage(existingRecipe.ImageName);
                }

                var imageName = await _imageService
                    .SaveImageAsync(recipeRequest.ImageData, recipeRequest.Name);
                existingRecipe.ImageName = imageName;
            }

            var updatedSteps = recipeRequest.Steps.Select(x => new Step()
            {
                Id= x.Id,
                RecipeId= recipeId,
                Description= x.Description,
                Order= x.Order,
            });

            var updatedIngs = recipeRequest.Ingredients.Select(x => new Ingredient()
            {
                Id = x.Id,
                RecipeId = recipeId,
                Description = x.Description,
            });

            await UpdateIngredientsAsync(existingRecipe.Ingredients, updatedIngs);
            await UpdateStepsAsync(existingRecipe.Steps, updatedSteps);

            existingRecipe.Name = recipeRequest.Name;
            existingRecipe.CategoryId = recipeRequest.CategoryId;
            _unitOfWork.RecipeRepository.Update(existingRecipe);

            await _unitOfWork.SaveAsync();
            
            return _mapper.Map<RecipeResponse>(existingRecipe);
        }

        private async Task UpdateIngredientsAsync(
            IEnumerable<Ingredient> existingIngredients,
            IEnumerable<Ingredient> requestIngredients
            )
        {
            // ingredients that don't exist in the request will be removed
            _unitOfWork.IngredientsRepository.DeleteRange(
                existingIngredients.Where(
                    x => !requestIngredients.Any(y => y.Id == x.Id && y.Id != 0)
                    )
                );

            // Update existing ingredients
            _unitOfWork.IngredientsRepository
                .UpdateRange(requestIngredients.Where(x => x.Id != 0));

            // Add new ingredients those with id == 0
            await _unitOfWork.IngredientsRepository
                .AddRangeAsync(requestIngredients.Where(x => x.Id == 0));
        }

        private async Task UpdateStepsAsync(
            IEnumerable<Step> existingSteps,
            IEnumerable<Step> requestSteps
            )
        {
            // steps that don't exist in the request will be removed
            _unitOfWork.StepsRepository.DeleteRange(
                existingSteps.Where(
                    x => !requestSteps.Any(y => y.Id == x.Id && y.Id != 0)
                    )
                );

            // Update existing steps {id != 0}
            _unitOfWork.StepsRepository
                .UpdateRange(requestSteps.Where(x => x.Id != 0));

            // Add new steps those with id == 0
            await _unitOfWork.StepsRepository
                .AddRangeAsync(requestSteps.Where(x => x.Id == 0));
        }

        public async Task<PaginatedList<RecipeSummary>> GetFavourites(PaginatedRequest request)
        {
            var recipes = await _unitOfWork.RecipeRepository
                .GetFavourites(_session.UserId, request.PageNumber, request.PageSize);

            return _mapper.Map<PaginatedList<RecipeSummary>>(recipes);
        }

        public async Task AddRecipeToFavourites(int recipeId)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            var favExists = await _unitOfWork.RecipeRepository
                .IsRecipeInFav(_session.UserId, recipeId);

            if (favExists)
            {
                throw new AlreadyInFavouritesException(recipeId);
            }

            await _unitOfWork.RecipeRepository
                .AddRecipeToFavourites(_session.UserId, recipeId);

            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRecipeFromFavourites(int recipeId)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            var favExists = await _unitOfWork.RecipeRepository
                .IsRecipeInFav(_session.UserId, recipeId);

            if (!favExists)
            {
                throw new RecipeNotInFavouritesException(recipeId);
            }

            await _unitOfWork.RecipeRepository
                .RemoveRecipeFromFavourites(_session.UserId, recipeId);
            await _unitOfWork.SaveAsync();
        }

        public async Task RemoveRecipeById(int recipeId)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            if (recipe.AuthorId != _session.UserId)
            {
                throw new UnAuthorizedException();
            }

            await _unitOfWork.RecipeRepository.DeleteById(recipeId);
            await _unitOfWork.SaveAsync();
        }

    }
}
