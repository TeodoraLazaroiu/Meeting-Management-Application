using MeetingManagement.Core.Common;

namespace MeetingManagement.Application.DTOs.Event
{
	public class CreateEventDTO
	{
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
        public List<Guid> Attendes { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public bool IsRecurring { get; set; }
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
        public ReccurenceType ReccurenceType { get; set; }
        public int? SeparationCount { get; set; }
        public int? NumberOfOccurences { get; set; }
        public List<int>? DaysOfWeek { get; set; }
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
    }
}

