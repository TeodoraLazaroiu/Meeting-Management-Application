using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserEntity> GetUserEntity(string id);
        Task<UserInfoDTO> GetUserInfo(string id);
        Task<List<UserEntity>> GetUserList();
        Task<string> RegisterUser(RegisterUserDTO registerUser);
        Task UpdateUser(string id, UpdateUserDTO updateUser);
        Task DeleteUser(string id);
    }
}
