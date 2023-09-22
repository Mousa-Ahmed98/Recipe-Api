using Core.Entities;
using System.ComponentModel.DataAnnotations;

namespace Recipe.DTOs.Request
{
    public class ReviewDto
    {
        
        [Required]
        public byte rate { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public string AuthorId { get; set; }
        [Required]
        public string AuthorName { get; set; }
        public int RecipeId { get; set; }

    }
}
