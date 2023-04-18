using MeetingManagement.Application.DTOs;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;

namespace MeetingManagement.Application.Services
{
    internal class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task RegisterUser(RegisterUserDTO registerUser)
        {
            var user = new UserEntity();

            user.Email = registerUser.Email;
            user.FirstName = registerUser.FirstName;
            user.LastName = registerUser.LastName;
            user.RoleTitle = registerUser.RoleTitle;

            if (registerUser.TeamId != null)
            {
                user.TeamId = new Guid(registerUser.TeamId);
            }
        }

        private (string, string) HashPassword(string password)
        {
            return ("", "");
        }
    }
}
