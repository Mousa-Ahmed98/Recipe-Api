using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Repositories.Implementation
{
    public class PlansRepository : BaseRepository<Plan>, IPlansRepository
    {
        public PlansRepository(StoreContext context) : base(context)
        {
        }

        public async Task<List<Plan>> GetAllPlans(string userId)
        {
            return await _context.Plans
                .Where(x => x.UserId == userId)
                .Include(x => x.Recipe)
                .ToListAsync();
        }

        public async Task<Plan> AddPlanAsync(string userId, DateTime day, int recipeId)
        {
            var plan = new Plan()
            {
                RecipeId = recipeId,
                UserId = userId,
                Day = day,
            };

            await _context.Plans.AddAsync(plan);
            return plan;
        }

    }
}
