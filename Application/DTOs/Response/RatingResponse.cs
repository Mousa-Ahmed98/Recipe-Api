using Core.CustomModels;

namespace Application.DTOs.Response
{
    public record RatingResponse
    {
        public int Id { get; set; }
        public UserResponse User { get; set; }
        public int NumberOfStars { get; set; }
        public string Content { get; set; }
    }
}
