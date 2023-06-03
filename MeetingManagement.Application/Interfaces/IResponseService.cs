using MeetingManagement.Application.DTOs.Response;

namespace MeetingManagement.Application.Interfaces
{
	public interface IResponseService
	{
        Task UpdateResponse(string userId, UserResponseDTO userResponse);
        Task<List<ResponseDetailsDTO>> GetUserResponses(string userId);
        Task<List<ResponseDetailsDTO>> GetResponsesByEvent(string eventId);
    }
}

