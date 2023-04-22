using MeetingManagement.Application.DTOs.User;

namespace MeetingManagement.Application.Interfaces
{
    public interface IAuthService
    {
        Task SignInUser(SignInUserDTO userCredentials);
        Task SignOutUser();
    }
}
