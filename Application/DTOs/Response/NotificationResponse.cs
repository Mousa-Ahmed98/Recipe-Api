using Core.Enums;
using Infrastructure.CustomModels;
using System;

namespace Application.DTOs.Response
{
    public class NotificationResponse
    {
        public DateTime CreatedAt { get; set; }
        public bool Read { get; set; }
        public RecipeSummary Recipe { get; set; }
        public NotificationType Type { get; set; }
    }
}
