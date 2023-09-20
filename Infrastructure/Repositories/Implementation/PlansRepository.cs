using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class PlansRepository : BaseRepository<Recipe>, IPlansRepository
    {
        public PlansRepository(StoreContext context) : base(context)
        {
        }

        public async Task<List<Plan>> GetAllPlans()
        {
            ThrowIfUserIdNull();
            
            return await _context.Plans
                .Where(x => x.UserId == _userId)
                .Include(x => x.Recipe)
                .ToListAsync();
        }

        public async Task<Plan> PlanOut(string day, int recipeId)
        {
            ThrowIfUserIdNull();

            var date = ParseDate(day);

            var plan = new Plan()
            {
                RecipeId = recipeId,
                UserId = _userId,
                Day = date,

            };  
            
            _context.Plans.Add(plan);
            await _context.SaveChangesAsync();

            return plan;
        }

        public async Task<bool> PlanOff(int planId)
        {
            ThrowIfUserIdNull();

            var plansForTheDay = await _context.Plans
                .Where(x => x.Id == planId)
                .FirstOrDefaultAsync();
            
            if (plansForTheDay == null)
            {
                // todo : thorw meaningful exception 
                return false;
            }

            if(plansForTheDay.UserId != _userId)
            {
                // todo : thorw meaningful exception 
                return false;
            }

            _context.Plans.RemoveRange(plansForTheDay);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ChangePlanDate(int planId, string date)
        {
            var plan = await _context.Plans.Where(x => x.Id == planId).FirstOrDefaultAsync();
            
            if (plan == null)
                // todo : throw meaning exception
                return false;

            if (plan.UserId != _userId)
                // todo : throw meaning exception
                return false;
            
            plan.Day = ParseDate(date);

            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SwitchPlans(string day1, string day2)
        {
            ThrowIfUserIdNull();

            var date1 = ParseDate(day1);
            var date2 = ParseDate(day2);

            var plansForDay1 = await _context.Plans
                .Where(x => x.UserId == _userId && x.Day == date1)
                .FirstOrDefaultAsync();
            
            var plansForDay2 = await _context.Plans
                .Where(x => x.UserId == _userId && x.Day == date2)
                .FirstOrDefaultAsync();

            if (plansForDay1 == null || plansForDay2 == null) return false;

            int temp = plansForDay1.RecipeId;
            plansForDay1.RecipeId = plansForDay2.RecipeId;
            plansForDay2.RecipeId = temp;

            return true;
        }

        private DateTime ParseDate(string inputDate)
        {
            bool success = DateTime.TryParseExact(
                inputDate,
                "yyyy-M-d",
                null,
                System.Globalization.DateTimeStyles.None,
                out DateTime parsedDate
                );
            
            if (!success)
            {
                // todo: throw exception
            }
            
            return parsedDate;
        }


    }
}
