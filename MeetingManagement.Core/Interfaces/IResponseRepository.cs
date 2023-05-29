using MeetingManagement.Core.Entities;

namespace MeetingManagement.Core.Interfaces
{
    public interface IResponseRepository : IGenericRepository<ResponseEntity>
    {
        Task<ResponseEntity?> GetResponseByUserAndEvent(string userId, string eventId);
        Task<List<ResponseEntity>> GetResponsesByUser(string userId);
    }
}
