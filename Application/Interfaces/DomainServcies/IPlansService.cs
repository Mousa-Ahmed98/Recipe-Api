using System.Collections.Generic;
using System.Threading.Tasks;
using Application.DTOs.Response;

namespace Application.Interfaces.DomainServices
{
    public interface IPlansService
    {
        Task<List<PlanResponse>> GetAllPlans();
        Task<PlanResponse> PlanOut(string day, int recipeId);
        Task PlanOff(int planId);
        Task ChangePlanDate(int planId, string day2);
    }
}