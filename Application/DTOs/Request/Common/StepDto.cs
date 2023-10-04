namespace Application.DTOs.Request.Common
{
    public record StepDto
    {
        public string Description { get; set; }
        public int Order { get; set; }
    }
}
