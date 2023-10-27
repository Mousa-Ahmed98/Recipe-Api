using System.Collections.Generic;
using System.Threading.Tasks;

using Core.Common;
using Core.CustomModels;
using Core.Entities;

namespace Core.Interfaces.Repositories
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
        Task<Recipe?> GetOneById(int id, string? userId);
        Task<PaginatedList<RecipeSummary>> GetRecipesSummary(string? userId, int pageNumber, int pageSize, string? category);
        Task<PaginatedList<RecipeSummary>> GetRecipesByAuthorId(string? userId, string authorId, int pageNumber, int pageSize);
        Task<PaginatedList<RecipeSummary>> FilterByIngredients(string? userId, int pageNumber, int pageSize, List<string> ingredients);
        Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, int pageNumber, int pageSize);

        Task<bool> IsRecipeInFav(string? userId, int recipeId);
        Task AddRecipeToFavourites(string? userId, int recipeId);
        Task RemoveRecipeFromFavourites(string? userId, int recipeId);
        Task<PaginatedList<RecipeSummary>> GetFavourites(string? userId, int pageNumber, int pageSize);
    }
}
