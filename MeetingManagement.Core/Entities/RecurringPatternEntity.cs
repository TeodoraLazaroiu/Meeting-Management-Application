using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class RecurringPatternEntity : AuditableEntity
    {
        public ReccurenceType ReccurenceType { get; set; }
        public int? SeparationCount { get; set; }
        public List<int>? DaysOfWeek { get; set; }
        public int? DayOfWeek { get; set; }
        public int? DayOfMonth { get; set; }
    }
}
