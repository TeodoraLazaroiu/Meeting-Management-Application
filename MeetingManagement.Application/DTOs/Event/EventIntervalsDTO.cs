namespace MeetingManagement.Application.DTOs.Event
{
	public class EventIntervalsDTO
	{
        public int StartTime { get; set; }
        public int EndTime { get; set; }

        public EventIntervalsDTO(int start, int end)
        {
            StartTime = start;
            EndTime = end;
        }
    }
}

