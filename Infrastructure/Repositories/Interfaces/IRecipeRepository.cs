﻿using Core.Entities;
using Core.Interfaces;
using Infrastructure.CustomModels;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IRecipeRepository : IBaseRepository<Recipe>
    {
        Task<PaginatedList<RecipeSummary>> GetRecipesSummary(int pageNumber, int pageSize, string? category);
        public Task<IEnumerable<Recipe>> GetAllRecipes();
        public Task<Recipe?> GetOneById(int id);
    }
}