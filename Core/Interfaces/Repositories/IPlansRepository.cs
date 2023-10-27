using Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IPlansRepository : IBaseRepository<Plan>
    {
        Task<List<Plan>> GetAllPlans(string userId);
        Task<Plan> AddPlanAsync(string userId, DateTime day, int recipeId);
    }
}
