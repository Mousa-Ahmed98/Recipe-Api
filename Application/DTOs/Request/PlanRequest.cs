using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Controllers
{
    public class PlanRequest
    {
        [Required]
        public string Day { get; set; }
        [Required]
        public int RecipeId { get; set; }
    }
}