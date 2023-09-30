using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApi.DTOs.Request.Common
{
    public record IngredientDto
    {
        [NotMapped]
        public int Id { get; set; }
        public required string Description { get; set; }
        //public required byte Order{ get; set; }
    }
}
