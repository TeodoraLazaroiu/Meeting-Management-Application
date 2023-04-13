using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class RecurringPattern : BaseEntity
    {
        public ReccurenceType ReccurenceType { get; set; }
        public int? SeparationCount { get; set; }
        public int? NumberOfOccurences { get; set; }
        public List<int>? DaysOfWeek { get; set; }
        public int? WeekOfMonth { get; set; }
        public int? DayOfMonth { get; set; }
    }
}
