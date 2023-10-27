namespace Application.DTOs.Common
{
    public record StepDto
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
