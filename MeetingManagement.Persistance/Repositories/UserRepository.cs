using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;
using MeetingManagement.Persistance.Context;

namespace MeetingManagement.Persistance.Repositories
{
    internal class UserRepository : GenericRepository<UserEntity>, IUserRepository
    {
        public UserRepository(IMongoDbContext context) : base(context) { }
    }
}
