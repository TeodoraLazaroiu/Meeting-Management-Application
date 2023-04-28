using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class EventExceptionRepository : GenericRepository<EventExceptionEntity>, IEventExceptionRepository
    {
        public EventExceptionRepository(IMongoDbContext context) : base(context) { }
    }
}
