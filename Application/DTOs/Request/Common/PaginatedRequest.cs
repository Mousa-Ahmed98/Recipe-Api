namespace Application.DTOs.Request.Common
{
    public record PaginatedRequest
    {
        public int CurrentPage { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
