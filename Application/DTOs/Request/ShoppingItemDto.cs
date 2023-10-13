using System.ComponentModel;

namespace Application.DTOs.Request
{
    public record ShoppingItemDto
    {
        public string Ingredient { get; set; }
        public int Quantity { get; set; }
        [DefaultValue(false)]
        public bool isPurchased { get; set; }
        public string UserId { get; set; }
    }
}
