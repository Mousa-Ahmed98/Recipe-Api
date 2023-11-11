using System.Threading.Tasks;
using AutoMapper;

using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Core.Common;
using Core.CustomModels;

using Infrastructure.Exceptions.User;
using Infrastructure.Exceptions.Recipe;

using Application.DTOs.Response;
using Application.DTOs.Request;
using Application.Interfaces;
using Application.Interfaces.DomainServcies;

namespace Application.Services.DomainServices
{
    public class RatingsService : IRatingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserSession _session;
        private readonly INotificationManager _notificationManager;

        public RatingsService(
            IMapper mapper,
            IUnitOfWork unitOfWork,
            IUserSession session,
            INotificationManager notificationManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _session = session;
            _notificationManager = notificationManager;
        }

        public async Task<PaginatedList<RatingResponse>> GetRatings
            (int recipeId, PaginatedRequest request)
        {
            PaginatedList<Rating> res = await _unitOfWork.RatingRepository
                .GetByRecipeId(recipeId, request.PageNumber, request.PageSize);
            
            return _mapper.Map<PaginatedList<RatingResponse>>(res);
        }

        public async Task<RatingResponse> AddRating(int recipeId, RatingRequest request)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);
            
            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            var alreadyRated = await _unitOfWork.RatingRepository
                .RatedAlready(_session.UserId, recipe.Id);

            if(alreadyRated)
            {
                throw new RecipeAlreadyRated(recipeId);
            }

            var rating = new Rating()
            {
                Content = request.Content,
                NumberOfStars = request.NumberOfStars,
                RecipeId = recipeId,
                UserId = _session.UserId,
            };

            await UpdateRecipeRating(
                recipeId: recipeId,
                ratingNumber: 1,
                value: rating.NumberOfStars
            );

            await _unitOfWork.RatingRepository.AddAsync(rating);
            await _unitOfWork.SaveAsync();

            // Notify the author about the new rating they've just recieved.
            _ = _notificationManager.CreateOne(
                    userId: recipe.AuthorId,
                    recipe: _mapper.Map<RecipeSummary>(recipe),
                    type:   NotificationType.Rating
                );

            return _mapper.Map<RatingResponse>(rating);
        }

        public async Task RemoveRating(int recipeId)
        {
            var rating = await _unitOfWork.RatingRepository.GetByUserId(_session.UserId, recipeId);
            
            if(rating == null)
            {
                throw new RatingNotFoundException(recipeId);
            }
            
            await UpdateRecipeRating(
                recipeId: rating.RecipeId,
                ratingNumber: -1,
                value: -rating.NumberOfStars 
            );

            _unitOfWork.RatingRepository.Delete(rating);
            await _unitOfWork.SaveAsync();
        }

        public async Task<RatingResponse> UpdateRating(int recipeId, RatingRequest request)
        {
            var rating = await _unitOfWork.RatingRepository.GetByUserId(_session.UserId, recipeId);

            if (rating == null)
            {
                throw new RatingNotFoundException(recipeId);
            }

            await UpdateRecipeRating(
                recipeId: rating.RecipeId, 
                ratingNumber : 0, 
                value: request.NumberOfStars - rating.NumberOfStars
                );
            
            rating.NumberOfStars = request.NumberOfStars;
            rating.Content = request.Content;

            _unitOfWork.RatingRepository.Update(rating);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<RatingResponse>(rating);
        }

        private async Task UpdateRecipeRating(int recipeId, int ratingNumber, int value)
        {
            var recipe = await _unitOfWork.RecipeRepository.GetByIdAsync(recipeId);
            
            recipe.NumberOfRatings += ratingNumber; // add 1, update 0, delete -1
            
            recipe.TotalRatings += value; // 1 : 5 to be added or decreased. and in case of update (new - old)
            
            recipe.AverageRating = ( recipe.NumberOfRatings > 0 ) ?
                (double)recipe.TotalRatings / recipe.NumberOfRatings : 5;

            _unitOfWork.RecipeRepository.Update(recipe);
        }
    }
}
