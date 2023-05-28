namespace MeetingManagement.Application.Exceptions
{
    public class TeamDeletionException : Exception
    {
        public TeamDeletionException()
        {
        }

        public TeamDeletionException(string? message) : base(message)
        {
        }
    }
}

