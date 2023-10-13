using System;

namespace Application.DTOs.Response
{
    public record PlanSummaryResponse
    {
        public int Id { get; set; }
        public DateTime Day { get; set; }
    }
}
