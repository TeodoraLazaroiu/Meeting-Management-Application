using MeetingManagement.Core.Entities;

namespace MeetingManagement.Core.Interfaces
{
    public interface ITeamRepository : IGenericRepository<TeamEntity>
    {
        Task<TeamEntity?> GetTeamByAccessCode(string accessCode);
    }
}
