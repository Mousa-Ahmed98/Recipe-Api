using RecipeAPI.DTOs.Request.Common;

namespace Recipe.DTOs.Request
{
    public record FilteredRecipeRequest : PaginatedRequest
    {
        public string Ingredients { get; set; } = string.Empty;
    }
}
