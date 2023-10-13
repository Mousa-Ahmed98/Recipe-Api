using Core.CustomModels;
using Core.Enums;
using System;

namespace Application.DTOs.Response
{
    public record NotificationResponse
    {
        public DateTime CreatedAt { get; set; }
        public bool Read { get; set; }
        public RecipeSummary Recipe { get; set; }
        public NotificationType Type { get; set; }
    }
}
