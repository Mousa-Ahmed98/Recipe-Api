using RecipeAPI.DTOs.Request.Common;

namespace RecipeAPI.DTOs.Request
{
    public record GetRecipeRequest : PaginatedRequest
    {
        public string? Category { get; set; }
    }
}
