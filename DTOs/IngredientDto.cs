using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class IngredientDto
    {
        public int Id { get; set; }

        [Required] 
        public string Description { get; set; }
    }
}

