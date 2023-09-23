using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
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
