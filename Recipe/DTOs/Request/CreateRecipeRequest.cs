using Microsoft.IdentityModel.Tokens;

namespace Recipe.DTOs.Request
{
    public record CreateRecipeRequest
    {
        public required string Name { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public required ICollection<IngredientDto> Ingredients { get; set; }
        public required ICollection<StepDto> Steps { get; set; }

        public string Validata()
        {
            if (Name.IsNullOrEmpty()) return "Name is mandatory and must be at least 3 characters.";
            else if (CategoryId == 0) return "Category is mandatory and you must choose one.";
            else if (Ingredients.IsNullOrEmpty()) return "There must be at least one ingredient.";
            else if (Steps.IsNullOrEmpty()) return "There must be at least one step.";
            return "";
        }

        public void appendOrdersToSteps()
        {
            int i = 0;
            foreach (var step in Steps)
            {
                step.Order = i + 1; i++;
            }
        }
    }
}
