using Application.DTOs.Request.Common;

namespace Application.DTOs.Request
{
    public record GetRecipeRequest : PaginatedRequest
    {
        public string? Category { get; set; }
    }
}
