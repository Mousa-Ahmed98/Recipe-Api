// using AutoMapper;
// using Core.Entities;
// using Recipe.DTOs;
// using Recipe.DTOs.Request;
// using Recipe.DTOs.Response;

// namespace Recipe.Helpers
// {
//     public class RecipeMappingProfile : Profile
//     {
//         public RecipeMappingProfile()
//         {
//             CreateMap<CreateRecipeRequest, Core.Entities.Recipe>()
//                 .ForMember(dest => dest.Ingredients, opt => 
//                 opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.ToString()})))
//                 .ForMember(dest => dest.Steps, opt => 
//                 opt.MapFrom(src => src.Steps.Select(s => new StepDto { Step = s.Step, Order = s.Order })));
            
//             CreateMap<Core.Entities.Recipe, RecipeResponse>()
//                 .ForMember(dest => dest.ImageURL, opt =>
//                     opt.MapFrom(
//                         src => src.Image
//                         ));
                

//         }
//     }
// }

// using AutoMapper;
// using Core.Entities;
// using Recipe.DTOs;
// using Recipe.DTOs.Request;
// using Recipe.DTOs.Response;

// namespace Recipe.Helpers
// {
//     public class RecipeMappingProfile : Profile
//     {
//         public RecipeMappingProfile()
//         {
//             CreateMap<CreateRecipeRequest, Core.Entities.Recipe>()
//                 .ForMember(dest => dest.Ingredients, opt =>
//                     opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description })))
//                 .ForMember(dest => dest.Steps, opt =>
//                     opt.MapFrom(src => src.Steps.Select(s => new Step { Description = s.Step, Order = (byte)s.Order })));

//             CreateMap<UpdateRecipeRequest, Core.Entities.Recipe>() // Add this mapping
//                 .ForMember(dest => dest.Ingredients, opt =>
//                     opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description })))
//                 .ForMember(dest => dest.Steps, opt =>
//                     opt.MapFrom(src => src.Steps.Select(s => new Step { Description = s.Step, Order = (byte)s.Order })));

//             CreateMap<Core.Entities.Recipe, RecipeResponse>()
//                 .ForMember(dest => dest.ImageURL, opt =>
//                     opt.MapFrom(src => src.Image));
//         }
//     }
// }

using AutoMapper;
using Core.Entities;
using Recipe.DTOs;
using Recipe.DTOs.Request;
using Recipe.DTOs.Response;

namespace Recipe.Helpers
{
public class RecipeMappingProfile : Profile
{
    public RecipeMappingProfile()
    {
        CreateMap<CreateRecipeRequest, Core.Entities.Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients != null ? src.Ingredients.Select(i => new Ingredient { Description = i.Description }) : null))
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src =>
                src.Steps != null ? src.Steps.Select(s => new Step { Description = s.Step, Order = (byte)s.StepOrder }) : null));

        CreateMap<UpdateRecipeRequest, Core.Entities.Recipe>()
            .ForMember(dest => dest.Ingredients, opt => opt.MapFrom(src =>
                src.Ingredients != null ? src.Ingredients.Select(i => new Ingredient { Description = i.Description }) : null))
            .ForMember(dest => dest.Steps, opt => opt.MapFrom(src =>
                src.Steps != null ? src.Steps.Select(s => new Step { Description = s.Step, Order = (byte)s.StepOrder }) : null));

        CreateMap<Core.Entities.Recipe, RecipeResponse>()
            .ForMember(dest => dest.ImageURL, opt => opt.MapFrom(src => src.Image));
    }
}
}
