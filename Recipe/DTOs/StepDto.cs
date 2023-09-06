using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class StepDto
    {
        public required string Description { get; set; }
        public required byte Order{ get; set; }
    }
}
