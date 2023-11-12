using System;
using Core.CustomModels;

namespace Application.DTOs.Response
{
    public record ReplyResponse
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public UserResponse User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
