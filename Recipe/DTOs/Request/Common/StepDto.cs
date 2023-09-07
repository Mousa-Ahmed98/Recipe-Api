using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs.Request.Common
{
    public record StepDto
    {
        public string Description { get; set; }
        public byte Order { get; set; }
    }
}
