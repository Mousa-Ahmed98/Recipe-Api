using System;
using System.IO;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Quartz;

using Core.Entities;
using Core.Interfaces.Repositories;

using Infrastructure.Data;
using Infrastructure.Data.DBInitializer;
using Infrastructure.Repositories.Implementation;

using Application.UserSession;
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

// Register Infrasctructure's Repositories
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IPlansRepository, PlansRepository>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<INotificationsRepository, NotificationsRepository>();

builder.Services.AddScoped<IImageService, ImageService>();

// Configure Authentication
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserSession, Session>();
builder.Services.AddScoped<IAuthService, AuthService>();

//Configuring Jwt Bearer
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidIssuer = "https://localhost:3000",
        ValidAudience = "https://localhost:4200",
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes("anscna@@#as321AaAcEEE37$%**$#2ffr@#fvf^(gkoeLAD8")
            )
    };
});


// Configure Quartz.NET scheduler
builder.Services.AddQuartz(q =>
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
builder.Services.AddQuartzHostedService(options =>
{
    options.WaitForJobsToComplete = true;
    options.AwaitApplicationStarted = true;
});

// Add Automapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder =>
        {
            builder.WithOrigins("http://localhost:4200")
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

