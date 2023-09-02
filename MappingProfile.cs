using AutoMapper;
using Core.Entities;
using Recipe.DTOs.Request;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateRecipeRequest, Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src => src.Ingredients));
    }
}
