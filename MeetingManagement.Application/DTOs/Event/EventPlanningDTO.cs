using MeetingManagement.Core.Common;

namespace MeetingManagement.Application.DTOs.Event
{
	public class EventPlanningDTO
    {
        public List<Guid> Attendes { get; set; } = null!;
        public string Date { get; set; } = null!;
    }
}

