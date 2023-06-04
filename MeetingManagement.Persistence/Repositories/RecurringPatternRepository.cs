using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistence.Context;

namespace MeetingManagement.Persistence.Repositories
{
    internal class RecurringPatternRepository : GenericRepository<RecurringPatternEntity>, IRecurringPatternRepository
    {
        public RecurringPatternRepository(IMongoDbContext context) : base(context) { }
    }
}
