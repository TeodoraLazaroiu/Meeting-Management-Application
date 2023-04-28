using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class TeamRepository : GenericRepository<TeamEntity>, ITeamRepository
    {
        public TeamRepository(IMongoDbContext context) : base(context) { }
    }
}
