using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; } 
        public CategoryResponse Category { get; set; }
        public List<IngredientResponse> Ingredients { get; set; }
        public List<StepResponse> Steps { get; set; }
    }
}
