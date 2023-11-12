using Application.DTOs.Common;
using Core.CustomModels;
using Core.Entities;
using System.Collections.Generic;

namespace Application.DTOs.Response
{
    public record RecipeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; } 
        public bool InFavourites { get; set; }
        public PlanSummaryResponse Plan { get; set; }
        public CategoryResponse Category { get; set; }
        public List<IngredientDto> Ingredients { get; set; }
        public RatingResponse UserRating { get; set; }
        public List<RatingResponse> Ratings { get; set; }
        public List<CommentResponse> Comments { get; set; }
        public List<StepDto> Steps { get; set; }
        public UserResponse Author { get; set; }
        public int NumberOfRatings { get; set; }
        public double AverageRating { get; set; }
    }
}
