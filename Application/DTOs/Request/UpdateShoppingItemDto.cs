using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Request
{
    public class UpdateShoppingItemDto : ShoppingItemDto
    {
        public int Id { get; set; }
    }
}
