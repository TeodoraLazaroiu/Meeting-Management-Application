using MeetingManagement.Core.Common;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.User
{
    public class UserInfoDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? JobTitle { get; set; }
        public Guid? TeamId { get; set; }
        public RoleType Role { get; set; }

        public UserInfoDTO(UserEntity user)
        {
            Id = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            JobTitle = user.JobTitle;
            TeamId = user.TeamId;
            Role = user.Role;
        }
    }
}

