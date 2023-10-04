using System;
using Core.Enums;

namespace Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; } = null;
        public NotificationType Type { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
