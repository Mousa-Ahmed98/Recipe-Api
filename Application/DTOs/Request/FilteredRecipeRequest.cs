namespace Application.DTOs.Request
{
    public record FilteredRecipeRequest : PaginatedRequest
    {
        public string Ingredients { get; set; } = string.Empty;
    }
}
