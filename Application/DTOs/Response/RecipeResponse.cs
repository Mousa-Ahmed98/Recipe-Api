using Application.DTOs.Request.Common;
using Infrastructure.Common;
using System.Collections.Generic;

namespace Application.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; } 
        public bool InFavourites { get; set; }
        public PlanSummaryResponse Plan { get; set; }
        public CategoryResponse Category { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }
        public UserResponse Author { get; set; }

    }
}
