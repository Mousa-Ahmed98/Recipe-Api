using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class StepDto
    {
        [Required] 
        public string Step { get; set; }

        [Required] 
        public int StepOrder { get; set; }
    }
}
