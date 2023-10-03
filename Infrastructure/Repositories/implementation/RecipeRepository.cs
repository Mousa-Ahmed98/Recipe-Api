using Core.Entities;
using Infrastructure.CustomModels;
using Infrastructure.Data;
using Infrastructure.Exceptions.Recipe;
using Infrastructure.Exceptions.User;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
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
            var query = _context.Recipes
                .Where(x => x.Id == id)
                .Include(x => x.Category)
                .Include(x => x.Steps)
                .Include(x => x.Ingredients)
                .Include(x => x.Plans
                    .Where(p => _userId != null && p.UserId == _userId)) // TODO :: refine this line later
                .Include(x => x.Author);

            var res = await query.FirstOrDefaultAsync();
            
            if (res == null) {
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

            if (recipe == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            _context.FavouriteRecipes.Add(new FavouriteRecipes
            {
                RecipeId = recipeId,
                UserId = _userId
            });

            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, int currentPage, int pageSize)
        {
            var recipes = _context.Recipes
                .Where(x => x.Name.Contains(query))
                .Select(x=> new RecipeSummary
                {
                    Id = x.Id,
                    ImageUrl = x.Image,
                    Name = x.Name,
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(recipes, currentPage, pageSize);
        }

        public async Task<bool> RemoveRecipeFromFavourites(int recipeId)
        {
            var fav = await _context.FavouriteRecipes
                .Where(x => x.RecipeId == recipeId && x.UserId == _userId)
                .FirstOrDefaultAsync();

            if (fav == null)
            {
                throw new RecipeNotFoundException(recipeId);
            }

            _context.FavouriteRecipes.Remove(fav);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<PaginatedList<RecipeSummary>> GetFavourites(int currentPage, int pageSize)
        {
            var query = _context.FavouriteRecipes
                .Where(fr => fr.UserId == _userId)
                .OrderByDescending(fr => fr.CreatedAt)
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

        public async Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(string username, int currentPage, int pageSize)
        {
            var user = _context.Users.Where(x => x.UserName== username).FirstOrDefault();
            
            if(user == null)
            {
                throw new UserNotFoundException(username);
            }

            var query = _context.Recipes
                .Where(r => r.AuthorId == user.Id)
                .Select(r => new RecipeSummary
                {
                    Id = r.Id,
                    Name = r.Name,
                    ImageUrl = r.Image,
                });
            
            return await PaginatedList<RecipeSummary>.CreateAsync(query, currentPage, pageSize);
        }

    }
}
