using System;
namespace MeetingManagement.Application.Exceptions
{
	public class EventDeletionException : Exception
	{
		public EventDeletionException()
		{

		}
		public EventDeletionException(string message) : base(message)
		{
		}
	}
}

