using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;

namespace MeetingManagement.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;

        public TeamService(ITeamRepository teamRepository)
        {
            _teamRepository = teamRepository;
        }

        public async Task<TeamEntity> CreateTeam(string userId, CreateTeamDTO teamDetails)
        {
            TeamEntity newTeam = new TeamEntity();

            newTeam.TeamName = teamDetails.TeamName;
            newTeam.CreatedBy = new Guid(userId);
            newTeam.AccessCode = GenerateTeamAccessCode();

            newTeam.CreatedDate = DateTime.UtcNow;
            newTeam.LastModified = DateTime.UtcNow;

            newTeam.Id = Guid.NewGuid();

            await _teamRepository.CreateAsync(newTeam);

            return newTeam;
        }

        private static string GenerateTeamAccessCode()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            string code = Convert.ToBase64String(bytes);

            code = code.Replace("=", "");
            code = code.Replace("+", "");

            return code;
        }
    }
}
