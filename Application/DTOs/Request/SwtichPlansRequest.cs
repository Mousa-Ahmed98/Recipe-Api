using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public record SwtichPlansRequest
    {
        [Required]
        public string Day1 { get; set; }

        [Required]
        public string Day2 { get; set; }
    }
}