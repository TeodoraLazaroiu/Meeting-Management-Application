namespace MeetingManagement.Application.DTOs.Team
{
    public class CreateTeamDTO
    {
        public string TeamName { get; set; } = null!;
        public int StartWorkingHour { get; set; }
        public int EndWorkingHour { get; set; }
    }
}
