using System;
using System.Collections.Generic;
using Core.CustomModels;

namespace Application.DTOs.Response
{
    public record CommentResponse
    {
        public int Id { get; set; }
        public UserResponse User { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public List<ReplyResponse> Replies { get; set; }
    }
}
