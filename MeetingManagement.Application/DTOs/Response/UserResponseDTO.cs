using MeetingManagement.Core.Entities;

namespace MeetingManagement.Application.DTOs.Response
{
	public class UserResponseDTO
	{
        public string EventId { get; set; } = null!;
        public bool? IsAttending { get; set; }
        public bool? SendReminder { get; set; }
        public int? ReminderTime { get; set; }

        public UserResponseDTO(ResponseEntity responseEntity)
        {
            EventId = responseEntity.EventId.ToString();
            IsAttending = responseEntity.IsAttending;
            SendReminder = responseEntity.SendReminder;
            ReminderTime = responseEntity.ReminderTime;
        }

        public UserResponseDTO()
        {

        }
    }
}

