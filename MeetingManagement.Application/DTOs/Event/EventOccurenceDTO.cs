using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.Event
{
	public class EventOccurenceDTO
    { 
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
        public List<Guid> Attendes { get; set; } = null!;
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public string Date { get; set; } = null!;

        public EventOccurenceDTO(EventEntity eventEntity, DateTime dateTime)
        {
            EventTitle = eventEntity.EventTitle;
            EventDescription = eventEntity.EventDescription;
            Attendes = eventEntity.Attendes;
            StartTime = eventEntity.StartTime.ToString();
            EndTime = eventEntity.EndTime.ToString();
            Date = DateOnly.FromDateTime(dateTime).ToString();
        }
    }
}

