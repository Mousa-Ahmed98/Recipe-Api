using System;
using Core.Enum;

namespace Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? ReadAt { get; set; } = null;
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public NotificationType Type { get; set; }

        public ApplicationUser User { get; set; }
        public string UserId { get; set; }
    }
}
