using Core.Entities;
using Infrastructure.Data;
using Infrastructure.Exceptions;
using Infrastructure.Exceptions.Plan;
using Infrastructure.Exceptions.Recipe;
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
            if (_userId == null) { 
                throw new UnAuthorizedException(); 
            }

            var date = ParseDate(day);

            var recipe = await _context.Recipes.FirstOrDefaultAsync(x => x.Id == recipeId);

            if(recipe == null) {
                throw new RecipeNotFoundException(recipeId);
            }

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
            if (_userId == null)
            {
                throw new UnAuthorizedException();
            }

            var plansForTheDay = await _context.Plans
                .Where(x => x.Id == planId)
                .FirstOrDefaultAsync();
            
            if (plansForTheDay == null)
            {
                throw new PlanNotFoundException(planId);
            }

            if(plansForTheDay.UserId != _userId)
            {
                throw new UnAuthorizedException();
            }

            _context.Plans.RemoveRange(plansForTheDay);
            await _context.SaveChangesAsync();
            
            return true;
        }

        public async Task<bool> ChangePlanDate(int planId, string date)
        {
            var plan = await _context.Plans.Where(x => x.Id == planId)
                .FirstOrDefaultAsync();
            
            if (plan == null)
            {
                throw new PlanNotFoundException(planId);
            }

            if (plan.UserId != _userId)
            {
                throw new UnAuthorizedException();
            }
            
            plan.Day = ParseDate(date);

            _context.Plans.Update(plan);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> SwitchPlans(string day1, string day2)
        {
            if (_userId == null)
            {
                throw new UnAuthorizedException();
            }

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
                throw new InvalidDateFormatException(inputDate);
            }
            
            return parsedDate;
        }


    }
}
