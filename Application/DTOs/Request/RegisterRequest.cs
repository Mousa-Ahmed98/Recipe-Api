﻿using System.ComponentModel.DataAnnotations;

namespace Core.Entities
{
    public record RegisterRequest
    {
        [Required]
        [MaxLength(24)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(24)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(14)]
        public string UserName { get; set; }
        
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}
