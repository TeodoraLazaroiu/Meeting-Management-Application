namespace MeetingManagement.Application.DTOs.Team
{
    public class CreateTeamDTO
    {
        public string TeamName { get; set; } = null!;
        public (int, int) WorkingHours { get; set; }
    }
}
