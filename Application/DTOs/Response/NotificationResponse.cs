using Core.Enum;
using System;

namespace Application.DTOs.Response
{
    public class NotificationResponse
    {
        public DateTime CreatedAt { get; set; }
        public bool Read { get; set; }
        public int RecipeId { get; set; }
        public NotificationType Type { get; set; }
    }
}
