using Application.DTOs.Request;

namespace Application.DTOs.Response
{
    public record ResponseShoppingItemDto : ShoppingItemDto
    {
        public int Id { get; set; }
    }
}
