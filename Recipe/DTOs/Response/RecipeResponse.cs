using Core.Entities;
using Recipe.DTOs.Request.Common;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; } 
        public int CategoryId { get; set; }

        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }

    }
}
