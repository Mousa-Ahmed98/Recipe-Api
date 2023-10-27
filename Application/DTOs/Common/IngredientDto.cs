namespace Application.DTOs.Common
{
    public record IngredientDto
    {
        public int Id { get; set; }
        public required string Description { get; set; }
    }
}
