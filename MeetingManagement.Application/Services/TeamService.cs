using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Common;
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

        public async Task<List<TeamEntity>> GetAllTeams()
        {
            return (await _teamRepository.GetAllAsync()).ToList();
        }

        public async Task<TeamDetailsDTO> GetTeamByUserId(string userId)
        {
            var user = await _userService.GetUserEntity(userId);
            var teamId = user.TeamId.ToString() ?? throw new TeamNotFoundException();

            TeamEntity? team;
            try
            {
                team = await _teamRepository.GetAsync(teamId);
            }
            catch (Exception)
            {
                throw new TeamNotFoundException();
            }

            if (team == null)
            {
                throw new TeamNotFoundException();
            }

            var teamDetails = new TeamDetailsDTO()
            {
                Id = team.Id,
                TeamName = team.TeamName,
                AccessCode = team.AccessCode,
                CreatedBy = team.CreatedBy,
                StartWorkingHour = team.StartWorkingHour,
                EndWorkingHour = team.EndWorkingHour
            };

            teamDetails.TeamMembers = await GetTeamMembers(teamId);
            return teamDetails;
        }

        private async Task<TeamEntity> GetTeamByAccessCode(string accessCode)
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
            newTeam.StartWorkingHour = teamDetails.StartWorkingHour;
            newTeam.EndWorkingHour = teamDetails.EndWorkingHour;

            newTeam.CreatedDate = DateTime.UtcNow;
            newTeam.LastModified = DateTime.UtcNow;

            newTeam.Id = Guid.NewGuid();

            await _teamRepository.CreateAsync(newTeam);

            var user = await _userService.GetUserEntity(userId);
            user.TeamId = newTeam.Id;
            user.TeamRole = RoleType.TeamAdmin;
            await _userRepository.UpdateAsync(user);

            return newTeam;
        }
        
        public async Task JoinTeam(string userId, string accessCode)
        {
            var user = await _userService.GetUserEntity(userId);

            var team = await GetTeamByAccessCode(accessCode);

            user.TeamId = team.Id;
            user.TeamRole = RoleType.TeamMember;

            await _userRepository.UpdateAsync(user);
        }

        private async Task<List<UserInfoDTO>> GetTeamMembers(string teamId)
        {
            var users = await _userRepository.GetUsersByTeamId(teamId);
            return users.Select(x => new UserInfoDTO(x)).ToList();
        }

        public async Task DeleteTeam(string userId)
        {
            var user = await _userService.GetUserEntity(userId);
            if (user.TeamRole != RoleType.TeamAdmin)
            {
                throw new TeamDeletionException("Only the team admin can perform this operation");
            }
            var team = await GetTeamByUserId(userId);
            if (team.TeamMembers.Count != 1)
            {
                throw new TeamDeletionException("The team must have no members before being deleted");
            }
            else await _teamRepository.DeleteAsync(team.Id.ToString());

            user.TeamRole = RoleType.NoTeam;
            user.TeamId = null;
            await _userRepository.UpdateAsync(user);
        }

        private static string GenerateTeamAccessCode()
        {
            Guid guid = Guid.NewGuid();
            byte[] bytes = guid.ToByteArray();
            string code = Convert.ToBase64String(bytes);

            code = code.Replace("=", "");
            code = code.Replace("+", "");
            code = code.Replace("/", "");

            return code;
        }
    }
}
