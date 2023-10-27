using System.Linq;
using AutoMapper;
using Core.Entities;
using Application.DTOs.Response;

using Application.DTOs.Request;
using Core.CustomModels;
using Core.Common;
using Application.DTOs.Common;

namespace RecipeApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            /// Recipe

            CreateMap<RecipeRequest, Recipe>()
                .ForMember(dest => dest.ImageName,
                    opt => opt.Ignore()
                    )
                .ForMember(dest => dest.Ingredients, opt =>
                    opt.MapFrom(src =>
                        src.Ingredients.Select(i => new Ingredient { Description = i.Description })
                    ))
                .ForMember(dest => dest.Steps, opt =>
                    opt.MapFrom(src =>
                        src.Steps.Select(s => new Step { Description = s.Description, Order = s.Order })
                    ));

            CreateMap<Recipe, RecipeResponse>()
                .ForMember(dest => dest.ImageName, opt =>
                    opt.MapFrom(src =>
                        src.ImageName
                    ))
                .ForMember(dest => dest.Plan, opt =>
                    opt.MapFrom(src =>
                        src.Plans.FirstOrDefault()
                    ))
                .ForMember(dest => dest.Ingredients, opt =>
                    opt.MapFrom(src =>
                        src.Ingredients.Select(i => new IngredientDto { Description = i.Description })
                    ))
                .ForMember(dest => dest.Steps, opt =>
                    opt.MapFrom(src =>
                        src.Steps.Select(s => new StepDto { Description = s.Description, Order = s.Order })
                    ))
                .ForMember(dest => dest.Category, opt =>
                    opt.MapFrom(src =>
                        src.Category
                    ))
                .ForMember(dest => dest.Author, opt =>
                    opt.MapFrom(src =>
                        src.Author
                    ));

            CreateMap<Recipe, RecipeSummary>();
            
            CreateMap<IngredientDto, Ingredient>();
            CreateMap<StepDto, Step>();

            CreateMap<Category, CategoryResponse>();

            CreateMap<StepDto, Step>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(
                        src => src.Description
                    ))
                .ForMember(dest => dest.Order,
                    opt => opt.MapFrom(src =>
                        src.Order
                    ));

            /// ApplicationUser

            CreateMap<ApplicationUser, UserResponse>();
            CreateMap<PaginatedList<ApplicationUser>, PaginatedList<UserResponse>>();

            /// Plans

            CreateMap<Plan, PlanResponse>();
            CreateMap<Plan, PlanSummaryResponse>();

            /// Notifications
            
            CreateMap<PaginatedList<Notification>, PaginatedList<NotificationResponse>>();

            CreateMap<Notification, NotificationResponse>()
                .ForMember(dest => dest.Read, opt =>
                    opt.MapFrom(src =>
                        src.ReadAt != null
                    ))
                .ForMember(dest => dest.Recipe, opt =>
                    opt.MapFrom(src =>
                        src.Recipe
                    ));

            
            /// ShoppingItem
            
            CreateMap<ShoppingItemDto, ShoppingItem>()
                .ForMember(dest => dest.Ingredient,
                opt => opt.MapFrom(src => src.Ingredient))
                .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.IsPurchased,
                opt => opt.MapFrom(src => src.isPurchased));

            CreateMap<ShoppingItem, ResponseShoppingItemDto>()
                .ForMember(dest => dest.Ingredient,
                opt => opt.MapFrom(src => src.Ingredient))
                .ForMember(dest => dest.UserId,
                opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Quantity,
                opt => opt.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.isPurchased,
                opt => opt.MapFrom(src => src.IsPurchased));
        }
    }
}
