using AutoMapper;
using Recipe.DTOs;

namespace Recipe.Helpers
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
