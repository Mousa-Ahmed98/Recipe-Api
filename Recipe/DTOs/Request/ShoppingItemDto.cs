using Core.Entities;
using System.ComponentModel;

namespace Recipe.DTOs.Request
{
    public class ShoppingItemDto
    {
        public string Ingredient { get; set; }
        public int Quantity { get; set; }
        [DefaultValue(false)]
        public bool isPurchased { get; set; }
        public string UserId { get; set; }

    }
}
