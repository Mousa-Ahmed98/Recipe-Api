using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

using Core.Entities;
using Core.Interfaces;

using Infrastructure.Data;
using Infrastructure.Data.DBInitializer;
using Infrastructure.Repositories.implementation;
using Infrastructure.Repositories.Interfaces;

using RecipeApi.Helpers;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<StoreContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly(typeof(StoreContext).Assembly.FullName)));

//Register UserManager
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<StoreContext>();

//Register the IBaseRepository and BaseRepository
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IAuthService, AuthService>();


// Add AutoMapper configuration in Startup.cs or a configuration file
builder.Services.AddAutoMapper(typeof(RecipeMappingProfile), typeof(StepMappingProfile));
              
builder.Services.AddCors(options =>
{
    options.AddPolicy("*",
        builder =>
        {
            builder.WithOrigins("http://localhost:4200") 
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition");
        });

});

//Configuring Jwt 
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

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("*");

app.MapControllers();

await app.InitDataAsync();

app.Run();

