// using Core.Interfaces;
// using Infrastructure.Data;
// using Infrastructure.Data.DBInitializer;
// using Infrastructure.Repositories;
// using Microsoft.EntityFrameworkCore;
// using Recipe.Helpers;

// namespace Recipe
// {
//     public class Program
//     {
//         public static async Task Main(string[] args)
//         {
//             var builder = WebApplication.CreateBuilder(args);

//             // Add services to the container.

//             builder.Services.AddControllers();
//             // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//             builder.Services.AddEndpointsApiExplorer();
//             builder.Services.AddSwaggerGen();

//             builder.Services.AddDbContext<StoreContext>(options
//                 => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
//                 b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

//             //Register the IBaseRepository and BaseRepository
//             builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

//             // Add AutoMapper configuration in Startup.cs or a configuration file
//             builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));

//             builder.Services.AddCors(opts =>
//             {
//                 opts.AddDefaultPolicy(builder =>
//                 {
//                     builder
//                         .WithOrigins("http://localhost:4200")
//                         .AllowAnyHeader()
//                         .AllowAnyMethod()
//                         .AllowCredentials();
//                 });
//             });

//             var app = builder.Build();

//             // Configure the HTTP request pipeline.
//             if (app.Environment.IsDevelopment())
//             {
//                 app.UseSwagger();
//                 app.UseSwaggerUI();
//             }

//             app.UseHttpsRedirection();
//             app.UseAuthorization();
//             app.MapControllers();
//             app.UseCors();

//             await app.InitDataAsync();

//             app.Run();

//         }
//     }
// }

using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Data.DBInitializer;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Recipe.Helpers;
using AutoMapper; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Recipe.DTOs.Request;
using Recipe.DTOs.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Recipe
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

            builder.Services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));

            builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));
            

            builder.Services.AddCors(opts =>
            {
                opts.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
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
            app.UseCors();

            await app.InitDataAsync();

            app.Run();
        }
    }
}
