namespace Application.DTOs.Request
{
    public record PaginatedRequest
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
    