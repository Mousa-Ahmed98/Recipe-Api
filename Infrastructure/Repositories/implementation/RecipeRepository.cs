using Core.Entities;
using Infrastructure.CustomModels;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;

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
                    includeProperties: "Category,Ingredients,Steps,Reviews"
                );
        }

        public async Task<Recipe?> GetOneById(int id)
        {
            var res = await GetAsync(
                    filter: r => r.Id == id,
                    includeProperties: "Category,Ingredients,Steps,Reviews"
                );
            
            return res.FirstOrDefault();
        }

        public async Task<PaginatedList<RecipeSummary>> GetRecipesSummary(
            int pageNumber, int pageSize, string? category = "Main"
            )
        {
            var query = _context.Recipes
                .Where(recipe => recipe.Category.Name == category)
                .OrderBy(recipe => recipe.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(recipe => new RecipeSummary
                {
                    Id = recipe.Id,
                    Name = recipe.Name,
                    ImageURL = recipe.Image
                });

            return await PaginatedList<RecipeSummary>.CreateAsync(query, pageNumber, pageSize);
        }
    }
}
