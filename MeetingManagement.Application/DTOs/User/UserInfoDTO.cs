using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.User
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? RoleTitle { get; set; }
        public Guid? TeamId { get; set; }

        public UserInfoDTO(UserEntity user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            RoleTitle = user.RoleTitle;
            TeamId = user.TeamId;
        }
    }
}

