using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class StepDto
    {
        public required string Step { get; set; }
        public required byte Order{ get; set; }
    }
}
