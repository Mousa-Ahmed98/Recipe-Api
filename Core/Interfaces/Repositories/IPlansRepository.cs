using Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IPlansRepository : IBaseRepository<Plan>
    {
        Task<List<Plan>> GetAllPlans();
        Task<bool> PlanOff(int planId);
        Task<Plan> PlanOut(string day, int recipeId);
        Task<bool> ChangePlanDate(int planId, string day2);
    }
}
