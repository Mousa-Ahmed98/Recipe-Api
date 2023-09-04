using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageURL { get; set; } 
        public Category Category { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<Step> Steps { get; set; }
    }
}
