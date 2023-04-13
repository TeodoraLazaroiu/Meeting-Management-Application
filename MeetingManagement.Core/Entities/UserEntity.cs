using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class UserEntity : AuditableEntity
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public string PasswordSalt { get; set; } = null!;
        public string? RoleTitle { get; set; }
        public Guid? TeamId { get; set; }

    }
}
