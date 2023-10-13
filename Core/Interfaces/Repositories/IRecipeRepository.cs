using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Common;
using Core.CustomModels;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
        Task<PaginatedList<RecipeSummary>> GetRecipesSummary(int pageNumber, int pageSize, string? category);
        Task<Recipe?> GetOneById(int id);
        Task<Recipe> CreateNewRecipe(Recipe recipe);
        Task RemoveRecipeById(int recipeId);
        Task<Recipe> UpdateRecipe(Recipe recipe);
        Task<PaginatedList<RecipeSummary>> FilterByIngredients(int pageNumber, int pageSize, List<string> ingredients);
        Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, int pageNumber, int pageSize);
        Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(string userId, string username, int currentPage, int pageSize);
        Task AddRecipeToFavourites(int recipeId);
        Task RemoveRecipeFromFavourites(int recipeId);
        Task<PaginatedList<RecipeSummary>> GetFavourites(string userId, int currentPage, int pageSize);
    }
}
