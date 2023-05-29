using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class ResponseEntity : AuditableEntity
    {
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
        public bool? IsAttending { get; set; }
        public bool? SendReminder { get; set; }
        public int? ReminderTime { get; set; }
    }
}
