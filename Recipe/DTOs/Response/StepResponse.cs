using System.ComponentModel.DataAnnotations;

namespace RecipeAPI.DTOs.Response
{
    public record StepResponse
    {
        public string Step { get; set; }
        public int Order { get; set; }
    }
}
