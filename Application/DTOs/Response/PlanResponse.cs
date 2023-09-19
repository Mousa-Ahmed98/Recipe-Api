using System;
using Infrastructure.CustomModels;

namespace Application.DTOs.Response
{
    public class PlanResponse
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
        public RecipeSummary Recipe { get; set; }
    }
}
