namespace Application.DTOs.Request
{
    public record UpdateShoppingItemDto : ShoppingItemDto
    {
        public int Id { get; set; }
    }
}
