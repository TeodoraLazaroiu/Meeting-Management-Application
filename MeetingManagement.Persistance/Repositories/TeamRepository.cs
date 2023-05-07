using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistance.Repositories
{
    internal class TeamRepository : GenericRepository<TeamEntity>, ITeamRepository
    {
        public TeamRepository(IMongoDbContext context) : base(context) { }

        public async Task<TeamEntity?> GetTeamByAccessCode(string accessCode)
        {
            var filter = Builders<TeamEntity>.Filter.Eq(x => x.AccessCode, accessCode);
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }
    }
}
