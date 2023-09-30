using Core.Entities;
using Microsoft.IdentityModel.Tokens;
using RecipeApi.DTOs.Request.Common;
using System.Collections.Generic;

namespace RecipeApi.DTOs.Request
{
    public record RecipeRequest
    {
        public required string Name { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public string AuthorId { get; set; }

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
            byte i = 0;
            foreach (var step in Steps)
            {
                step.Order = i++;
            }
        }

        public void applyUpdateChanges(Recipe recipe)
        {
            recipe.Name = Name ?? recipe.Name;
            recipe.CategoryId = CategoryId;
            recipe.Image = ImageUrl ?? recipe.Image;
            recipe.Ingredients = getUpdatedIngredients();
            recipe.Steps = getUpdatedSteps();
        }
        private List<Ingredient> getUpdatedIngredients()
        {
            List<Ingredient> updatedIngredients = new List<Ingredient>();
            foreach (var item in Ingredients)
            {
                updatedIngredients.Add(new Ingredient { Description = item.Description });
            }
            return updatedIngredients;
        }

        private List<Step> getUpdatedSteps()
        {
            byte i = 1;
            List<Step> updatedSteps = new List<Step>();
            foreach (var item in Steps)
            {
                updatedSteps.Add(new Step { Description = item.Description, Order = i});
                i++;
            }
            return updatedSteps;
        }


    }
}
