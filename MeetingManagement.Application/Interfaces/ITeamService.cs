using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
    public interface ITeamService
    {
        Task<TeamEntity> CreateTeam(string userId, CreateTeamDTO teamDetails);
        Task<TeamEntity> GetTeamByAccessCode(string accessCode);
        Task<TeamEntity> GetTeamById(string teamId);
        Task JoinTeam(string userId, string teamId);
        Task JoinTeam(string userId, Guid teamId);
    }
}
