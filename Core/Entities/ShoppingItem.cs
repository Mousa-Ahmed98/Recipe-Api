using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities
{
    public class ShoppingItem
    {
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public string Ingredient { get; set; }
        public int Quantity { get; set; }
        public bool IsPurchased { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
