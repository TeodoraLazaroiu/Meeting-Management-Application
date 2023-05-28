using System.ComponentModel.DataAnnotations;
using MeetingManagement.Core.Common;

namespace MeetingManagement.Application.DTOs.User
{
    public class RegisterUserDTO
    {
        [Required, RegularExpression("(?=^.{8,}$)(?=.*\\d)(?=.*\\W)(?=.*[A-Z])(?=.*[a-z]).*$")]
        public string Password { get; set; } = null!;
        [Required, EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
    }
}
