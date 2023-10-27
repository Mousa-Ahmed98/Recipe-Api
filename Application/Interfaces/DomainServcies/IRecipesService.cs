using System.Threading.Tasks;

using Core.Common;
using Core.CustomModels;

using Application.DTOs.Request;
using Application.DTOs.Response;


namespace Application.Interfaces.DomainServcies
{
    public interface IRecipesService
    {
        Task<RecipeResponse> GetOneById(int id);
        Task<PaginatedList<RecipeSummary>> GetRecipesSummary(GetRecipeRequest request);
        Task<PaginatedList<RecipeSummary>> GetRecipesByUsername(string username, PaginatedRequest request);
        Task<PaginatedList<RecipeSummary>> GetMyRecipesAsync(PaginatedRequest request);
        Task<PaginatedList<RecipeSummary>> FilterByIngredients(FilteredRecipeRequest request);
        Task<PaginatedList<RecipeSummary>> SearchRecipes(string query, PaginatedRequest request);

        Task<RecipeResponse> CreateNewRecipe(RecipeRequest recipeRequest);
        Task<RecipeResponse> UpdateRecipe(int id, RecipeRequest recipeRequest);
        Task RemoveRecipeById(int recipeId);
        
        Task AddRecipeToFavourites(int recipeId);
        Task RemoveRecipeFromFavourites(int recipeId);
        Task<PaginatedList<RecipeSummary>> GetFavourites(PaginatedRequest request);
    }
}
