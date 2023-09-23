using Application.DTOs.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Response
{
    public class ResponseShoppingItemDto : ShoppingItemDto
    {
        public int Id { get; set; }
    }
}
