using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Core.Entities;
using Quartz;

namespace Application.Services
{
    public class PlansReminder : IJob
    {
        private readonly IServiceProvider _serviceProvider;

        public PlansReminder(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public async Task Execute(IJobExecutionContext jobContext)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<StoreContext>();
                    var newNotifications = new List<Notification>();
                    var plansForTomorrow = await dbContext.Plans
                        .Where(x => x.Day == DateTime.Now.AddDays(1))
                        .ToListAsync();

                    foreach (var plan in plansForTomorrow)
                    {
                        newNotifications.Add(new Notification
                        {
                            UserId = plan.UserId,
                            RecipeId = plan.RecipeId,
                            Type = Core.Enum.NotificationType.PlanReminder,
                        });
                    }

                    await dbContext.Notifications.AddRangeAsync(newNotifications);
                    await dbContext.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
