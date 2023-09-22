using AutoMapper;
using Core.Entities;
using Recipe.DTOs.Request;
using Recipe.DTOs.Request.Common;
using Recipe.DTOs.Response;
using RecipeAPI.DTOs.Request;
using RecipeAPI.DTOs.Response;

namespace Recipe.Helpers
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {

            CreateMap<RecipeRequest, Core.Entities.Recipe>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))

                .ForMember(dest => dest.Ingredients, opt =>
               opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description })))
                .ForMember(dest => dest.Steps, opt =>
                opt.MapFrom(src => src.Steps.Select(s => new Step { Description = s.Description, Order = s.Order })));

                /*.ForMember(dest => dest.Reviews, opt =>
                opt.MapFrom(src => src.Reviews.Select(r => new Review { AuthorId = r.AuthorId, AuthorName = r.AuthorName, RecipeId = r.RecipeId, content = r.content, rate = r.rate })));*/



            CreateMap<Core.Entities.Recipe, RecipeResponse>()
                .ForMember(dest => dest.ImageUrl, opt =>
                    opt.MapFrom(
                        src => src.Image
                        ))
                .ForMember(dest => dest.Ingredients, opt =>
                opt.MapFrom(src => src.Ingredients.Select(i => new IngredientDto { Description = i.Description })))
                .ForMember(dest => dest.Steps, opt =>
                opt.MapFrom(src => src.Steps.Select(s => new StepDto { Description = s.Description, Order = s.Order })))
                .ForMember(dest => dest.Category, opt =>
                    opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Reviews, opt =>
                opt.MapFrom(src => src.Reviews.Select(r => new ReviewDto { rate = r.rate, content = r.content, RecipeId = r.RecipeId, AuthorName = r.AuthorName, AuthorId = r.AuthorId })));



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


            CreateMap<Category, CategoryResponse>();
            
        }
    }
}
