using AutoMapper;
using Core.Entities;
using Recipe.DTOs;

namespace Recipe.Helpers
{
    public class RecipeMappingProfile : Profile
    {
        public RecipeMappingProfile()
        {
            CreateMap<RecipeDto, Core.Entities.Recipe>()
                .ForMember(dest => dest.Ingredients, opt => 
                opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Ingrdnt = i})))
                .ForMember(dest => dest.Steps, opt => 
                opt.MapFrom(src => src.Steps.Select(s => new StepDto { Step = s.Step, Order = s.Order })));

        }
    }
}
