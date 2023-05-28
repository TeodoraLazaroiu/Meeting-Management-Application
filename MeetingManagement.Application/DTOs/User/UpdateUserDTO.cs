using System.ComponentModel.DataAnnotations;
using MeetingManagement.Core.Common;

namespace MeetingManagement.Application.DTOs.User
{
    public class UpdateUserDTO
    {
        [Required]
        public string FirstName { get; set; } = null!;
        [Required]
        public string LastName { get; set; } = null!;
        public string? JobTitle { get; set; }
    }
}
