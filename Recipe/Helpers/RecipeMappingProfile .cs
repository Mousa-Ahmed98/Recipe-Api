using AutoMapper;
using Core.Entities;
using RecipeAPI.DTOs.Request;
using RecipeAPI.DTOs.Response;

namespace Recipe.Helpers
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<CreateRecipeRequest, Core.Entities.Recipe>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Image))
                .ForMember(dest => dest.Ingredients, opt => 
                opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description})))
                .ForMember(dest => dest.Steps, opt => 
                opt.MapFrom(src => src.Steps.Select(s => new Step { Description = s.Description, Order = s.Order })));

            CreateMap<Core.Entities.Recipe, RecipeResponse>()
                .ForMember(dest => dest.ImageURL,
                    opt => opt.MapFrom(
                        src => src.Image));

            CreateMap<Category, CategoryResponse>();
            CreateMap<Step, StepResponse>();
            CreateMap<Ingredient, IngredientResponse>();


        }
    }
}
