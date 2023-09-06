using Core.Entities;

namespace RecipeAPI.DTOs.Request
{
    public record UpdateRecipeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
        public int CategoryId { get; set; }
    }
}
