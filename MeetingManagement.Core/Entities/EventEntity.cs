using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class EventEntity : AuditableEntity
    {
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
        public List<Guid> Attendes { get; set; } = null!;
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public bool IsRecurring { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsRescheduled { get; set; }
        public Guid? ParentEventId { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
