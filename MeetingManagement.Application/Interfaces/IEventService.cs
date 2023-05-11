using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
	public interface IEventService
	{
		Task CreateEvent(string userId, CreateEventDTO eventDetails);
		Task<List<EventEntity>> GetEventsForUser(string userId);

    }
}

