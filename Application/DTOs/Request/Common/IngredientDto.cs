using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request.Common
{
    public record IngredientDto
    {
        //public int Id { get; set; }
        public required string Description { get; set; }
        //public required byte Order{ get; set; }
    }
}
