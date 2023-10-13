using Core.CustomModels;
using System;

namespace Application.DTOs.Response
{
    public record PlanResponse
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public RecipeSummary Recipe { get; set; }
    }
}
