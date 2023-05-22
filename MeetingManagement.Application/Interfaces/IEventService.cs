using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
	public interface IEventService
	{
		Task CreateEvent(string userId, CreateEventDTO eventDetails);
		Task<List<EventEntity>> GetEventsForUser(string userId);
		Task<List<EventEntity>> GetEventsForTeam(string userId);
		Task<List<EventIntervalsDTO>> GenerateEventIntervals(string userId, EventPlanningDTO eventPlan);

    }
}

