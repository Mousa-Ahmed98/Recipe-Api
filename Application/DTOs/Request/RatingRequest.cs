using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Request
{
    public record RatingRequest
    {
        public int RecipeId { get; set; }
        [Range(0, 5)]
        public int NumberOfStars { get; set; }
        public string Content { get; set; }
    }
}
