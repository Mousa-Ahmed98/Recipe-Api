using Application.DTOs.Common;
using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;

namespace Application.DTOs.Request
{
    public record RecipeRequest
    {
        public required string Name { get; set; }
        public string? ImageData { get; set; }
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

    }
}
