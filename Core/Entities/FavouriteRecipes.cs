using System;

namespace Core.Entities
{
    public class FavouriteRecipes
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
