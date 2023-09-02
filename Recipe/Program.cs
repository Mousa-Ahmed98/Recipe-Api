//using Core.Interfaces;
//using Infrastructure.Data;
//using Infrastructure.Repositories;
//using Microsoft.EntityFrameworkCore;
//using Recipe.Helpers;

//namespace Recipe
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            var builder = WebApplication.CreateBuilder(args);

//            // Add services to the container.

//            builder.Services.AddControllers();
//            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//            builder.Services.AddEndpointsApiExplorer();
//            builder.Services.AddSwaggerGen();

//            builder.Services.AddDbContext<StoreContext>(options
//                => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//                b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

//            //Register the IBaseRepository and BaseRepository
//            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//            // Add AutoMapper configuration in Startup.cs or a configuration file
//            builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));


//            var app = builder.Build();

//            // Configure the HTTP request pipeline.
//            if (app.Environment.IsDevelopment())
//            {
//                app.UseSwagger();
//                app.UseSwaggerUI();
//            }

//            app.UseHttpsRedirection();

//            app.UseAuthorization();


//            app.MapControllers();

//            app.Run();
//        }
//    }
//}

using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Recipe.Helpers;

namespace Recipe
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Configure DbContext
            builder.Services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

            // Configure AutoMapper and register mapping profiles
            builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));

            // Register the IBaseRepository and BaseRepository
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            // Build the application
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

            app.Run();
        }
    }
}
