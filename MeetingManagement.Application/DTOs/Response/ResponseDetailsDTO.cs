using System;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.Response
{
	public class ResponseDetailsDTO
	{
        public string EventTitle { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string EventId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string? UserEmail { get; set; }
        public bool? IsAttending { get; set; }
        public bool? SendReminder { get; set; }
        public int? ReminderTime { get; set; }

        public ResponseDetailsDTO(ResponseEntity response, EventEntity eventEntity, string userId, string? userEmail = null)
        {
            EventTitle = eventEntity.EventTitle;
            StartDate = eventEntity.StartDate.ToString("dd/MM/yyyy");
            EndDate = eventEntity.EndDate.ToString("dd/MM/yyyy");
            StartTime = eventEntity.StartTime.ToString();
            EndTime = eventEntity.EndTime.ToString();
            EventId = response.EventId.ToString();
            UserId = userId;
            UserEmail = userEmail;
            IsAttending = response.IsAttending;
            SendReminder = response.SendReminder;
            ReminderTime = response.ReminderTime;
        }
    }
}

