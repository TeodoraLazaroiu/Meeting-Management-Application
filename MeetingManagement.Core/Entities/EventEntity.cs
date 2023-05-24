using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class EventEntity : AuditableEntity
    {
        public string EventTitle { get; set; } = null!;
        public string EventDescription { get; set; } = null!;
        public List<Guid> Attendes { get; set; } = null!;
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsRecurring { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool? IsCancelled { get; set; }
        public bool? IsRescheduled { get; set; }
        public Guid? ParentEventId { get; set; }
        public Guid CreatedBy { get; set; }

    }
}
