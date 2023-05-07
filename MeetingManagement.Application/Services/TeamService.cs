using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;

namespace MeetingManagement.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;

        public TeamService(ITeamRepository teamRepository, IUserService userService, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userService = userService;
            _userRepository = userRepository;
        }

        public async Task<TeamEntity> GetTeamById(string teamId)
        {
            try
            {
                var team = await _teamRepository.GetAsync(teamId);
                return team ?? throw new TeamNotFoundException();
            }
            catch (Exception)
            {
                throw new TeamNotFoundException();
            }
        }
        
        public async Task<TeamEntity> GetTeamByAccessCode(string accessCode)
        {
            try
            {
                var team = await _teamRepository.GetTeamByAccessCode(accessCode);
                return team ?? throw new TeamNotFoundException();
            }
            catch (Exception)
            {
                throw new TeamNotFoundException();
            }
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
            await JoinTeam(userId, newTeam.Id.ToString());

            return newTeam;
        }

        public async Task JoinTeam(string userId, Guid teamId)
        {
            var user = await _userService.GetUserEntity(userId);

            user.TeamId = teamId;

            await _userRepository.UpdateAsync(user);
        }
        
        public async Task JoinTeam(string userId, string accessCode)
        {
            var user = await _userService.GetUserEntity(userId);

            var team = await GetTeamByAccessCode(accessCode);

            user.TeamId = team.Id;

            await _userRepository.UpdateAsync(user);
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
