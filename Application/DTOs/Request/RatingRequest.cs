﻿namespace Application.DTOs.Request
{
    public record RatingRequest
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int NumberOfStars { get; set; }
        public string Content { get; set; }
    }
}
