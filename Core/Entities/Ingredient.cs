using System.ComponentModel.DataAnnotations;
namespace Core.Entities
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
