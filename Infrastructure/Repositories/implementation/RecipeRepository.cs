﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core.Interfaces.Repositories;
using Core.CustomModels;
using Core.Common;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Recipe;
using Infrastructure.Exceptions.User;

namespace Infrastructure.Repositories.Implementation
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(StoreContext context) : base(context)
        {
        }

        public async Task<Recipe?> GetOneById(int id)
        {
            var query = _context.Recipes
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Steps)
                .Include(x => x.Ingredients)
                .Include(x => x.Plans
                    .Where(p => _userId != null && p.UserId == _userId)) // TODO :: refine this line later
                .Include(x => x.Author);

            var res = await query.FirstOrDefaultAsync();

            if (res == null)
            {
                throw new RecipeNotFoundException(id);
            }

            res.InFavourites = _userId != null && _context.FavouriteRecipes
                .Any(f => f.RecipeId == res.Id && f.UserId == _userId);

            return res;
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesSummary(
            int pageNumber, int pageSize, string? category = "Main"
            )
        {
            var query = _context.Recipes.AsQueryable();

            if (!string.IsNullOrEmpty(category))
            {
                var cat = await _context.Categories
                    .Where(c => c.Name == category)
                    .FirstOrDefaultAsync();

                if (cat != null)
                {
                    query = query.Where(recipe => recipe.CategoryId == cat.Id);
                }
            };

            var res = query
                .OrderBy(recipe => recipe.Id)
                .Select(recipe => new RecipeSummary
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    ImageName = recipe.ImageName,
                    InFavourites = _userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == recipe.Id && f.UserId == _userId)
                });

            return await PaginatedList<RecipeSummary>
                .CreateAsync(res, pageNumber, pageSize);
        }

        public async Task<PaginatedList<RecipeSummary>> FilterByIngredients(
            int pageNumber, int pageSize, List<string> filterIngredients
             )
        {
            if (!filterIngredients.Any())
            {
                return await GetRecipesSummary(pageNumber, pageSize, null);
            }

            var query = _context.Ingredients.AsQueryable();
            var filteredIngredients = new List<Ingredient>();

            foreach (var ing in filterIngredients)
            {
                var filteredQuery = query.Where(x => x.Description.Contains(ing));
                // getting ingredients here one by one 
                // TODO :: try to make it one call
                filteredIngredients.AddRange(await filteredQuery.ToListAsync());
            }

            // group ingredients by their recipeId
            var groupedIngredients = filteredIngredients
                .Distinct()
                .GroupBy(x => x.RecipeId);

            // get all recipe ids that contain all (same number of) provided filter ingredients
            var filteredRecipeIds = groupedIngredients
                .Where(g => g.Count() >= filterIngredients.Count)
                .Select(g => g.Key)
                .ToList();
            
            // select recipe summary based on ids we filtered in filteredRecipeIds
            var filteredRecipes = _context.Recipes
                .Where(r => filteredRecipeIds.Contains(r.Id))
                .Distinct()
                .Select(x => new RecipeSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageName = x.ImageName,
                    InFavourites = _userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == x.Id && f.UserId == _userId)
                });

            return await PaginatedList<RecipeSummary>.CreateAsync
                (filteredRecipes, pageNumber, pageSize);
        }

        public async Task AddRecipeToFavourites(int recipeId)
        {
            bool recipeExists = await _context.Recipes
                .AnyAsync(r => r.Id == recipeId);

            if (!recipeExists)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            var favExists = await _context.FavouriteRecipes
                .AnyAsync(f => f.RecipeId == recipeId && f.UserId == _userId);

            if (favExists)
            {
                throw new AlreadyInFavouritesException(recipeId);
            }

            _context.FavouriteRecipes.Add(new FavouriteRecipes
            {
                RecipeId = recipeId,
                UserId = _userId
            });

            await _context.SaveChangesAsync();
        }


        public async Task RemoveRecipeFromFavourites(int recipeId)
        {
            var fav = await _context.FavouriteRecipes
                .FirstOrDefaultAsync(f => f.RecipeId == recipeId && f.UserId == _userId);

            if (fav == null)
            {
                throw new RecipeNotInFavouritesException(recipeId);
            }

            _context.FavouriteRecipes.Remove(fav);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginatedList<RecipeSummary>> GetFavourites(string userId, int currentPage, int pageSize)
        {
            var query = _context.FavouriteRecipes
                .Where(fr => fr.UserId == userId)
                .OrderByDescending(fr => fr.CreatedAt)
                .Join(_context.Recipes, fr => fr.RecipeId, r => r.Id, (fr, r) => r)
                .Select(r => new RecipeSummary
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageName = r.ImageName,
                    InFavourites = true
                });

            return await PaginatedList<RecipeSummary>
                .CreateAsync(query, currentPage, pageSize);
        }


        public async Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, int currentPage, int pageSize)
        {
            var recipes = _context.Recipes
                .Where(x => x.Name.Contains(query))
                .Select(x => new RecipeSummary
                {
                    Id = x.Id,
                    ImageName = x.ImageName,
                    Name = x.Name,
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(recipes, currentPage, pageSize);
        }


        public async Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(string userId, string username, int currentPage, int pageSize)
        {
            var user = _context.Users.Where(x => x.UserName == username).FirstOrDefault();

            if (user == null)
            {
                throw new UserNotFoundException(username);
            }

            var query = _context.Recipes
                .Where(r => r.AuthorId == user.Id)
                .Select(r => new RecipeSummary
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageName = r.ImageName,
                    InFavourites = userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == r.Id && f.UserId == userId)

                });

            return await PaginatedList<RecipeSummary>.CreateAsync(query, currentPage, pageSize);
        }

        public async Task<Recipe> CreateNewRecipe(Recipe recipe)
        {
            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();
            
            await NotifyFollowers(recipe.Id, recipe.AuthorId);
            
            return recipe;
        }

        private async Task NotifyFollowers(int recipeId, string authorId)
        {
            var followers = await _context.Follows
                .Where(x => x.FolloweeId == authorId)
                .Join(_context.Users,
                    f => f.FollowerId,
                    u => u.Id,
                    (f, u) => u
                ).ToListAsync();

            foreach (var f in followers)
            {
                await _context.Notifications.AddAsync(
                    new Notification()
                    {
                        RecipeId = recipeId,
                        UserId = f.Id,
                        Type = Core.Enums.NotificationType.NewPost,
                    }
                );
            }
         
            await _context.SaveChangesAsync();
        }

        public async Task RemoveRecipeById(int recipeId)
        {
            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(x => x.Id == recipeId);

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            if (recipe.AuthorId != _userId)
            {
                throw new UnAuthorizedException();
            }

            _context.Recipes.Remove(recipe);

            await _context.SaveChangesAsync();
        }

        public async Task<Recipe> UpdateRecipe(Recipe request)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.Id == request.CategoryId);

            if (category == null)
            {
                throw new NotFoundException("Category Not found");
            }

            request.Category = category;

            _context.Ingredients.RemoveRange(
                _context.Ingredients.Where(x => x.RecipeId == request.Id)
                );
        
            _context.Steps.RemoveRange(
                _context.Steps.Where(x => x.RecipeId == request.Id)
            );

            _context.Recipes.Update(request);
            await _context.SaveChangesAsync();
            
            return request;
        }
    }
}
