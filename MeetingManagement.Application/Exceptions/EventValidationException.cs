using System;
namespace MeetingManagement.Application.Exceptions
{
	public class EventValidationException : Exception
	{
		public EventValidationException(string message) : base(message) { }
    }
}

