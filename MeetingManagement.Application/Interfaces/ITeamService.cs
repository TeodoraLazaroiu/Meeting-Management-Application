using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
    public interface ITeamService
    {
        Task<TeamEntity> CreateTeam(string userId, CreateTeamDTO teamDetails);
        Task<TeamDetailsDTO> GetTeamByUserId(string userId);
        Task<List<TeamEntity>> GetAllTeams();
        Task JoinTeam(string userId, string accessCode);
        Task DeleteTeam(string userId);
    }
}
