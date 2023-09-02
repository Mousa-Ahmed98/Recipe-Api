using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public required string Description { get; set; }

        public static implicit operator string(IngredientDto v)
        {
            throw new NotImplementedException();
        }
    }
}
