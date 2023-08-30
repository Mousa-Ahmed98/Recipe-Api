using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json.Serialization;

namespace Recipe.DTOs
{
    public class RecipeDto
    {
        public required string Name { get; set; }
        public string? Image { get; set; }

        public required ICollection<string> Ingredients { get; set; }
        public required ICollection<StepDto> Steps { get; set; }

        
        public string Validata()
        {
            if(Name.IsNullOrEmpty()) return "Name is mandatory and must be at least 3 characters.";
            else if(Ingredients.IsNullOrEmpty()) return "There must be at least one ingredient.";
            else if(Steps.IsNullOrEmpty()) return "There must be at least one step.";
            return "";
        }
    }
}
