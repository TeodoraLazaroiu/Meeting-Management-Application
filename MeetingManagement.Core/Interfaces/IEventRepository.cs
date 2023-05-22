using MeetingManagement.Core.Entities;

namespace MeetingManagement.Core.Interfaces
{
    public interface IEventRepository : IGenericRepository<EventEntity>
    {
        Task<List<EventEntity>> GetEventsByUserId(string userId);
        Task<List<EventEntity>> GetEventsForUserIds(List<Guid> userIds);
    }
}
