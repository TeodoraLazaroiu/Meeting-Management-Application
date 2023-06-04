using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistence.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistence.Repositories
{
    internal class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IMongoDbContext context) : base(context) { }

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Email, email);
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }

        public async Task<List<UserEntity>> GetUsersByTeamId(string teamId)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.TeamId, new Guid(teamId));
            return await _dbCollection.Find(filter).ToListAsync();
        }
    }
}
