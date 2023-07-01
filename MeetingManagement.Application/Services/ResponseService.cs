using MeetingManagement.Application.DTOs.Response;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;

namespace MeetingManagement.Application.Services
{
	public class ResponseService : IResponseService
	{
		private readonly IResponseRepository _responseRepository;
		private readonly IEventRepository _eventRepository;
		private readonly IUserRepository _userRepository;

		public ResponseService(IResponseRepository responseRepository, IEventRepository eventRepository, IUserRepository userRepository)
		{
			_responseRepository = responseRepository;
			_eventRepository = eventRepository;
			_userRepository = userRepository;
		}

		private async Task<ResponseEntity> GetResponseEntity(string userId, string eventId)
		{
            try
            {
				var response = await _responseRepository.GetResponseByUserAndEvent(userId, eventId);

                return response ?? throw new ResponseNotFoundException();
            }
            catch (Exception)
            {
                throw new ResponseNotFoundException();
            }
        }

        public async Task<ResponseDetailsDTO> GetResponseByIds(string userId, string eventId)
        {
            var response = await GetResponseEntity(userId, eventId);
            var eventEntity = await _eventRepository.GetAsync(eventId) ?? throw new EventNotFoundException();

            return new ResponseDetailsDTO(response, eventEntity, userId);
        }

        public async Task<List<ResponseDetailsDTO>> GetResponsesByEvent(string eventId)
        {
            var responses = await _responseRepository.GetResponsesByEvent(eventId);
            var responsesDetails = new List<ResponseDetailsDTO>();

            foreach (var response in responses)
            {
                var eventEntity = await _eventRepository.GetAsync(response.EventId.ToString())
					?? throw new EventNotFoundException();
				var userEntity = await _userRepository.GetAsync(response.UserId.ToString())
					?? throw new UserNotFoundException();
                if (eventEntity == null) throw new EventNotFoundException();
                responsesDetails.Add(new ResponseDetailsDTO(response, eventEntity, userEntity.Id.ToString(), userEntity.Email));
            }

            return responsesDetails;
        }

        public async Task<List<ResponseDetailsDTO>> GetUserResponses(string userId)
		{
			var responses = await _responseRepository.GetResponsesByUser(userId);
			var responsesDetails = new List<ResponseDetailsDTO>();

			foreach (var response in responses)
			{
				var eventEntity = await _eventRepository.GetAsync(response.EventId.ToString());
				if (eventEntity == null) throw new EventNotFoundException();
				responsesDetails.Add(new ResponseDetailsDTO(response, eventEntity, userId));
            }
			
			return responsesDetails;
		}

		public async Task UpdateResponse(string userId, UserResponseDTO userResponse)
		{
			if (userResponse.IsAttending == false)
			{
				if (userResponse.SendReminder == true || (userResponse.ReminderTime != null && userResponse.ReminderTime != 0))
					throw new ResponseValidationException();
			}
			else
			{
				if (userResponse.SendReminder == true && (userResponse.ReminderTime == null || userResponse.ReminderTime == 0))
                    throw new ResponseValidationException();
                if (userResponse.SendReminder == false && userResponse.ReminderTime != null && userResponse.ReminderTime != 0)
                    throw new ResponseValidationException();
            }

			var response = await GetResponseEntity(userId, userResponse.EventId);
			response.IsAttending = userResponse.IsAttending;
			response.SendReminder = userResponse.SendReminder;
			response.ReminderTime = userResponse.ReminderTime;
			response.LastModified = DateTime.UtcNow;

			await _responseRepository.UpdateAsync(response);
		}
	}
}

