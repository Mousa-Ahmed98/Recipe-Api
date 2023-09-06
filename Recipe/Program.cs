using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DBInitializer;
using Infrastructure.Repositories;
using Infrastructure.Repositories.implementation;
using Infrastructure.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Recipe.Helpers;

namespace Recipe
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options
                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

            //Register the IBaseRepository and BaseRepository
            builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Add AutoMapper configuration in Startup.cs or a configuration file
            builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));
              
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("*",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200") // Add your Angular app's origin(s)
                       .AllowAnyHeader()
                       .AllowAnyMethod()
                       .AllowCredentials()
                       .WithExposedHeaders("Content-Disposition"); // If needed for file downloads
                    });

            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.UseCors("*");

            await app.InitDataAsync();

            app.Run();

        }
    }
}