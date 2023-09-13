using Core.Entities;
using Infrastructure.CustomModels;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Numerics;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.implementation
{
    public class RecipeRepository : BaseRepository<Recipe>, IRecipeRepository
    {
        public RecipeRepository(StoreContext context) : base(context)
        {
        }
        
        public async Task<IEnumerable<Recipe>> GetAllRecipes()
        {
            return await GetAsync(
                    includeProperties: "Category,Ingredients,Steps"
                );
        }

        public async Task<Recipe?> GetOneById(int id)
        {
            var res = (await GetAsync(
                    filter: r => r.Id == id,
                    includeProperties: "Category,Ingredients,Steps"
                )).FirstOrDefault();

            if(res == null) { return null; }
            
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
                
                if(cat!= null)
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
                ImageUrl = recipe.Image,
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
                filteredIngredients.AddRange(await filteredQuery.ToListAsync());
            }
            
            var groupedIngredients = filteredIngredients
                .Distinct()
                .GroupBy(x => x.RecipeId);

            var filteredRecipeIds = groupedIngredients
                .Where(g => g.Count() >= filterIngredients.Count)
                .Select(g => g.Key)
                .ToList();

            var filteredRecipes = _context.Recipes
                .Where(r => filteredRecipeIds.Contains(r.Id))
                .Distinct()
                .Select(x => new RecipeSummary
                {
                    Id = x.Id,
                    Name = x.Name,
                    ImageUrl = x.Image,
                    InFavourites = _userId != null && _context.FavouriteRecipes
                        .Any(f => f.RecipeId == x.Id && f.UserId == _userId)
                });

            return await PaginatedList<RecipeSummary>.CreateAsync
                (filteredRecipes, pageNumber, pageSize);
        }

        public async Task<bool> AddRecipeToFavourites(int recipeId)
        {
            Recipe? recipe = await _context.Recipes.Where(r => r.Id == recipeId)
                .FirstOrDefaultAsync();
            
            if (recipe == null) return false;

            _context.FavouriteRecipes.Add(new FavouriteRecipes
            {
                RecipeId = recipeId,
                UserId = _userId
            });

            await _context.SaveChangesAsync();
            
            return true;
        }
        public async Task<bool> RemoveRecipeFromFavourites(int recipeId)
        {
            var fav = await _context.FavouriteRecipes
                .Where(x => x.RecipeId == recipeId && x.UserId == _userId)
                .FirstOrDefaultAsync();
            
            if(fav == null) return false;

            _context.FavouriteRecipes.Remove(fav);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedList<RecipeSummary>> GetFavourites(int currentPage, int pageSize)
        {
            var query = _context.FavouriteRecipes
                .Where(fr => fr.UserId == _userId)
                .Join(_context.Recipes, fr => fr.RecipeId, r => r.Id, (fr, r) => r)
                .Select(r => new RecipeSummary
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.Image,
                    InFavourites = true
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(query, currentPage, pageSize);

        }
    }
}
