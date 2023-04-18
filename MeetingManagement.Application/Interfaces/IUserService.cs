using MeetingManagement.Application.DTOs;

namespace MeetingManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUser(RegisterUserDTO registerUser);
    }
}
