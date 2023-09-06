﻿using AutoMapper;
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
            CreateMap<RecipeRequest, Core.Entities.Recipe>()
                .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.Ingredients, opt => 
                opt.MapFrom(src => src.Ingredients.Select(i => new Ingredient { Description = i.Description})))
                .ForMember(dest => dest.Steps, opt => 
                opt.MapFrom(src => src.Steps.Select(s => new StepDto { Description = s.Description, Order = s.Order })));
            
            CreateMap<Core.Entities.Recipe, RecipeResponse>()
                .ForMember(dest => dest.ImageUrl, opt =>
                    opt.MapFrom(
                        src => src.Image
                        ));
                

        }
    }
}
