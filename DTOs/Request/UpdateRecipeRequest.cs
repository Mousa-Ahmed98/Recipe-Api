using Core.Entities;
namespace Recipe.DTOs.Request
{
    public record UpdateRecipeRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public List<StepDto> Steps { get; set; }
        public int CategoryId { get; set; }
    }
}
