using System.ComponentModel.DataAnnotations;

namespace MeetingManagement.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? RoleTitle { get; set; }
    }
}
