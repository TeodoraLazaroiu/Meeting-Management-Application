using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class EventRepository : GenericRepository<EventEntity>, IEventRepository
    {
        public EventRepository(IMongoDbContext context) : base(context) { }
    }
}
