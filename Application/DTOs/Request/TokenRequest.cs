using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public record TokenRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
