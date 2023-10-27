using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using Core.Entities;
using Core.Interfaces.Repositories;
using Core.CustomModels;
using Core.Common;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(StoreContext context) : base(context) { }

        public async Task<Recipe?> GetOneById(int id, string? userId)
        {
            var query = _context.Recipes
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Steps)
                .Include(x => x.Ingredients)
                .Include(x => x.Plans
                    .Where(p => userId != null && p.UserId == userId))
                .Include(x => x.Author);

            var res = await query.FirstOrDefaultAsync();

            if (res == null)
            {
                return null;
            }

            res.InFavourites = userId != null && _context.FavouriteRecipes
                .Any(f => f.RecipeId == res.Id && f.UserId == userId);

            return res;
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesSummary(
            string? userId, int pageNumber, int pageSize, string? category = "Main"
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
                    InFavourites = userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == recipe.Id && f.UserId == userId)
                });

            return await PaginatedList<RecipeSummary>
                .CreateAsync(res, pageNumber, pageSize);
        }

        public async Task<PaginatedList<RecipeSummary>> FilterByIngredients(
            string? userId, int pageNumber, int pageSize, List<string> filterIngredients
             )
        {
            if (!filterIngredients.Any())
            {
                return await GetRecipesSummary(userId, pageNumber, pageSize, null);
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
                    InFavourites = userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == x.Id && f.UserId == userId)
                });

            return await PaginatedList<RecipeSummary>.CreateAsync
                (filteredRecipes, pageNumber, pageSize);
        }

        public async Task<PaginatedList<RecipeSummary>> SearchRecipes
            (string query, int pageNumber, int pageSize)
        {
            var recipes = _context.Recipes
                .Where(x => x.Name.Contains(query))
                .Select(x => new RecipeSummary
                {
                    Id = x.Id,
                    ImageName = x.ImageName,
                    Name = x.Name,
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(recipes, pageNumber, pageSize);
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesByAuthorId
            (string? userId, string authorId, int pageNumber, int pageSize)
        {
            var query = _context.Recipes
                .Where(r => r.AuthorId == authorId)
                .Select(r => new RecipeSummary
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageName = r.ImageName,
                    InFavourites = userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == r.Id && f.UserId == userId)
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(query, pageNumber, pageSize);
        }

        public async Task AddRecipeToFavourites(string? userId, int recipeId)
        {
            await _context.FavouriteRecipes.AddAsync(new FavouriteRecipes
            {
                RecipeId = recipeId,
                UserId = userId
            });
        }

        public async Task RemoveRecipeFromFavourites(string? userId, int recipeId)
        {
            var fav = await _context.FavouriteRecipes
                .FirstOrDefaultAsync(f => f.RecipeId == recipeId && f.UserId == userId);

            _context.FavouriteRecipes.Remove(fav);
        }

        public async Task<PaginatedList<RecipeSummary>> GetFavourites
            (string? userId, int pageNumber, int pageSize)
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
                .CreateAsync(query, pageNumber, pageSize);
        }

        public async Task<bool> IsRecipeInFav(string? userId, int recipeId)
        {
            return await _context.FavouriteRecipes
                .AnyAsync(x => x.UserId == userId && x.RecipeId == recipeId);
        }

    }
}
