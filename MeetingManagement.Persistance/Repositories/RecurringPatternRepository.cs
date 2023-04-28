using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class RecurringPatternRepository : GenericRepository<RecurringPatternEntity>, IRecurringPatternRepository
    {
        public RecurringPatternRepository(IMongoDbContext context) : base(context) { }
    }
}
