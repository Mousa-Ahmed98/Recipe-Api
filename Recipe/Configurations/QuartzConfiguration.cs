using System;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Application.Services;

namespace RecipeApi.Configurations
{
    public static class QuartzConfiguration
    {
        public static void ConfigureQuartz(this IServiceCollection services) {
            
            // Configure Quartz.NET scheduler
            services.AddQuartz(q =>
            {
                // Add the job and specify the job type
                q.AddJob<PlansReminder>(j =>
                    j.WithIdentity(nameof(PlansReminder), "group1")
                    );

                // Configure the trigger for the job
                q.AddTrigger(t => t
                    .ForJob(nameof(PlansReminder), "group1")
                    .StartNow()
                    .WithSimpleSchedule(s => s
                        .WithInterval(TimeSpan.FromDays(1)) // everyday 
                        .RepeatForever()));
            });

            // Configure the Quartz.NET hosted service
            services.AddQuartzHostedService(options =>
            {
                options.WaitForJobsToComplete = true;
                options.AwaitApplicationStarted = true;
            });

        }
    }
}
