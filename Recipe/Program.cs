using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quartz;

using Core.Entities;
using Core.Interfaces;

using Infrastructure.Data;
using Infrastructure.Data.DBInitializer;
using Infrastructure.UnitOfWork.Implementation;

using Application.Services;
using Application.Interfaces;

using RecipeApi.Mappings;
using RecipeApi.Configurations;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)
    ));

// configure and add Identity services
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<StoreContext>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddDomainServices();

// configure authentication
builder.Services.ConfigureAuthentication(builder.Configuration);

// configure Quartz.NET
builder.Services.ConfigureQuartz();

// Add Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

var origins = builder.Configuration.GetSection("CorsOrigins").Value;
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins(origins!)
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images")
        ),
    RequestPath = "/images"
});

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.InitDataAsync();

app.Run();

