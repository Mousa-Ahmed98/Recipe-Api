using AutoMapper;
using RecipeApi.DTOs.Request.Common;

namespace RecipeApi.Helpers
{
    public class StepMappingProfile : Profile
    {
        public StepMappingProfile()
        {
            CreateMap<StepDto, Core.Entities.Step>()
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Order, opt => opt.MapFrom(src => src.Order));
        }
    }
}
