using Application.DTOs.Request;
using Application.DTOs.Response;
using AutoMapper;
using Core.Entities;
using Infrastructure.Common;
using Infrastructure.CustomModels;
using RecipeApi.DTOs.Request;
using RecipeApi.DTOs.Request.Common;
using RecipeAPI.DTOs.Request;
using RecipeAPI.DTOs.Response;
using System.Linq;

namespace RecipeApi.Helpers
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {

            CreateMap<RecipeRequest, Recipe>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))

                .ForMember(dest => dest.Ingredients, opt =>
               opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description })))
                .ForMember(dest => dest.Steps, opt =>
                opt.MapFrom(src => src.Steps.Select(s => new Step { Description = s.Description, Order = s.Order })));

            CreateMap<Recipe, RecipeResponse>()
                .ForMember(dest => dest.ImageUrl, opt =>
                    opt.MapFrom(
                        src => src.Image
                        ))
                .ForMember(dest => dest.Plan, opt =>
                    opt.MapFrom(
                        src => src.Plans.FirstOrDefault()
                        ))
                .ForMember(dest => dest.Ingredients, opt =>
                opt.MapFrom(src => src.Ingredients.Select(i => new IngredientDto { Description = i.Description })))
                .ForMember(dest => dest.Steps, opt =>
                opt.MapFrom(src => src.Steps.Select(s => new StepDto { Description = s.Description, Order = s.Order })))
                .ForMember(dest => dest.Category, opt =>
                    opt.MapFrom(src => src.Category));



            CreateMap<ShoppingItemDto, ShoppingItem>()
                .ForMember(dest => dest.Ingredient,
                opt => opt.MapFrom(src => src.Ingredient))
                .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.isPurchased,
                opt => opt.MapFrom(src => src.isPurchased));



            CreateMap<ShoppingItem, ResponseShoppingItemDto>()
                .ForMember(dest => dest.Ingredient,
                opt => opt.MapFrom(src => src.Ingredient))
                .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.isPurchased,
                opt => opt.MapFrom(src => src.isPurchased));





            CreateMap<ApplicationUser, UserResponse>();

            CreateMap<Recipe, RecipeSummary>()
                .ForMember(dest => dest.ImageUrl, opt =>
                    opt.MapFrom(
                        src => src.Image
                        ));

            CreateMap<Plan, PlanResponse>();
            CreateMap<Plan, PlanSummaryResponse>();

            CreateMap<Category, CategoryResponse>();
            
        }
    }
}
