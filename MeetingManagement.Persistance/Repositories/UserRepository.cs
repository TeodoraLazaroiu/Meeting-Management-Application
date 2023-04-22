using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;
using MongoDB.Driver;

namespace MeetingManagement.Persistance.Repositories
{
    internal class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IMongoDbContext context) : base(context) { }

        public async Task<UserEntity?> GetUserByEmail(string email)
        {
            var filter = Builders<UserEntity>.Filter.Eq(x => x.Email, email);
            return await _dbCollection.Find(filter).SingleOrDefaultAsync();
        }
    }
}
