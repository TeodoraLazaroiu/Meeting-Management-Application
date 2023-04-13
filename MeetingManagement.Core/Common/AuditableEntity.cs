namespace MeetingManagement.Core.Common
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
    }
}
