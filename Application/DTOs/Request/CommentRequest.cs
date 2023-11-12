namespace Application.DTOs.Request
{
    public record CommentRequest
    {
        public int RecipeId { get; set; }
        public string Content { get; set; }
    }
}
