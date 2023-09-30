using Application.DTOs.Request;
using Application.DTOs.Response;
using Core.Entities;
using RecipeApi.DTOs.Request.Common;
using System.Collections.Generic;

namespace RecipeAPI.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } 
        public bool InFavourites { get; set; }
        public PlanSummaryResponse Plan { get; set; }
        public CategoryResponse Category { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }
        public List<ReviewDto> Reviews { get; set; }

    }
}
