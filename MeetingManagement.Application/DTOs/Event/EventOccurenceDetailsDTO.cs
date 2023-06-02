using System;
using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.Event
{
	public class EventDetailsDTO
	{
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
        public List<UserInfoDTO> AttendesInfo { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public bool IsRecurring { get; set; }
        public UserInfoDTO CreatedBy { get; set; } = null!;
    }
}

