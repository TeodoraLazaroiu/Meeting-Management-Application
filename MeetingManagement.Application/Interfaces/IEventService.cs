using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
	public interface IEventService
	{
		Task CreateEvent(string userId, CreateEventDTO eventDetails);
		Task<EventDetailsDTO> GetEventById(string id);
		Task<List<EventOccurenceDTO>> GetEvents(int year = 0, int month = 0, int day = 0);
        Task<List<EventOccurenceDTO>> GetEventsForUser(string userId, int year, int month, int day);
		Task<List<EventOccurenceDTO>> GetEventsForTeam(string userId, int year, int month, int day);
		Task DeleteEvent(string userId, string eventId);
        Task<List<EventIntervalsDTO>> GenerateEventIntervals(string userId, EventPlanningDTO eventPlan);

    }
}

