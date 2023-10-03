using System;

namespace Core.Entities
{
    public class Plan
    {
        public int Id { get; set; } 
        public DateTime Day { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
