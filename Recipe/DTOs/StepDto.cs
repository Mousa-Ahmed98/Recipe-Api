using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs
{
    public class StepDto
    {
        public required string Step { get; set; }
        public required int Order{ get; set; }
    }
}
