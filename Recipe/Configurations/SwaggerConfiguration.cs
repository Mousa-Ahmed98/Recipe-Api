using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace RecipeApi.Configurations
{
    public static class SwaggerConfiguration
    {
        public static void AddSwagger(this IServiceCollection services) {

            services.AddSwaggerGen(options =>
            {
                options.DescribeAllParametersInCamelCase();
                options.OrderActionsBy(x => x.RelativePath);

                // add JWT bearer authorization 
                // https://stackoverflow.com/a/58667736

                OpenApiSecurityScheme securityDefinition = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    BearerFormat = "JWT",
                    Scheme = "bearer",
                    Description = "Specify the authorization token.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };

                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityDefinition);

                // Make sure swagger UI requires a Bearer token specified
                OpenApiSecurityScheme JwtScheme = new OpenApiSecurityScheme()
                {
                    Reference = new OpenApiReference()
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                OpenApiSecurityRequirement securityRequirements = new OpenApiSecurityRequirement()
                {
                    {JwtScheme, new string[] { }},
                };

                options.AddSecurityRequirement(securityRequirements);
            });
        }
    }
}
