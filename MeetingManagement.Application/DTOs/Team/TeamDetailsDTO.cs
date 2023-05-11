using MeetingManagement.Application.DTOs.User;

namespace MeetingManagement.Application.DTOs.Team
{
	public class TeamDetailsDTO
	{
        public Guid Id { get; set; }
        public string TeamName { get; set; } = null!;
        public string AccessCode { get; set; } = null!;
        public Guid CreatedBy { get; set; }
        public List<UserInfoDTO> TeamMembers { get; set; } = null!;
    }
}

