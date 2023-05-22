using MeetingManagement.Core.Common;

namespace MeetingManagement.Application.DTOs.Event
{
	public class EventPlanningDTO
    {
        public List<Guid> Attendes { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string StartDate { get; set; } = null!;
        public string EndDate { get; set; } = null!;
    }
}

