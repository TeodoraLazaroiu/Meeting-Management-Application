using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.Interfaces
{
    public interface ITeamService
    {
        Task<TeamEntity> CreateTeam(string userId, CreateTeamDTO teamDetails);
    }
}
