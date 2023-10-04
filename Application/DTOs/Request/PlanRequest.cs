using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public class PlanRequest
    {
        [Required]
        public string Day { get; set; }
        [Required]
        public int RecipeId { get; set; }
    }
}