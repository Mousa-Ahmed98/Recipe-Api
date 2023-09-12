using Core.Entities;
using Core.Interfaces;
using Infrastructure.CustomModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
        Task<PaginatedList<RecipeSummary>> GetRecipesSummary(int pageNumber, int pageSize, string? category);
        Task<PaginatedList<RecipeSummary>> FilterByIngredients(int pageNumber, int pageSize, List<string> ingredients);
        Task<IEnumerable<Recipe>> GetAllRecipes();
        Task<Recipe?> GetOneById(int id);
    }
}
