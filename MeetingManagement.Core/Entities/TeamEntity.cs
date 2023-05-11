using MeetingManagement.Core.Common;

namespace MeetingManagement.Core.Entities
{
    public class TeamEntity : AuditableEntity
    {
        public string TeamName { get; set; } = null!;
        public string AccessCode { get; set; } = null!;
        public Guid CreatedBy { get; set; }
    }
}
