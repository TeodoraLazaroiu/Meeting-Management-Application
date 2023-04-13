using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class EventExceptionEntity : AuditableEntity
    {
        public Guid EventId { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsRescheduled { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public Guid CreatedBy { get; set; }
    }
}
