using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
	public interface IEventService
	{
		Task CreateEvent(string userId, CreateEventDTO eventDetails);
		Task<List<EventOccurenceDTO>> GetEventsForUser(string userId, int year, int month, int day);
		Task<List<EventOccurenceDTO>> GetEventsForTeam(string userId, int year, int month, int day);
		Task<List<EventIntervalsDTO>> GenerateEventIntervals(string userId, EventPlanningDTO eventPlan);

    }
}

