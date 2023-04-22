using MeetingManagement.Core.Entities;

namespace MeetingManagement.Core.Interfaces
{
    public interface IUserRepository : IGenericRepository<UserEntity>
    {
        Task<UserEntity> GetUserByEmail(string email);
    }
}
