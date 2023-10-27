using Microsoft.Extensions.DependencyInjection;
using Application.Interfaces.DomainServcies;
using Application.Interfaces.DomainServices;
using Application.Services.DomainServices;

namespace RecipeApi.Configurations
{
    public static class DomianServicesConfiguration
    {
        public static void AddDomainServices(this IServiceCollection services) {
            services.AddScoped<IRecipesService, RecipesService>();
            services.AddScoped<ICategoriesService, CategoriesService>();
            services.AddScoped<IPlansService, PlansService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<INotificationsService, NotificationsService>();
            services.AddScoped<IRatingsService, RatingsService>();
        }
    }
}
