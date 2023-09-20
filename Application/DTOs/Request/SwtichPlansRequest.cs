using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Controllers
{
    public class SwtichPlansRequest
    {
        [Required]
        public string Day1 { get; set; }

        [Required]
        public string Day2 { get; set; }
    }
}