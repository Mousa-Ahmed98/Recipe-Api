using Core.Entities;
using Core.Interfaces;
using Infrastructure.CustomModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Interfaces
{
    public interface IPlansRepository : IBaseRepository<Recipe>
    {
        Task<List<Plan>> GetAllPlans();
        Task<bool> PlanOff(int planId);
        Task<Plan> PlanOut(string day, int recipeId);
        Task<bool> ChangePlanDate(int planId, string day2);
    }
}
