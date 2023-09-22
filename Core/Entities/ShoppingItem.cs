using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        
        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }
        public string Ingredient { get; set; }
        public int Quantity { get; set; }
        public bool isPurchased { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
