using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.DTOs.Response
{
    public record IngredientResponse
    {
        public int Id { get; set; }
        public required string Description { get; set; }
    }
}
