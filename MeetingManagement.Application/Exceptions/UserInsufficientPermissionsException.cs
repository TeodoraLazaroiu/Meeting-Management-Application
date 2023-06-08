using System;
namespace MeetingManagement.Application.Exceptions
{
	public class UserInsufficientPermissionsException : Exception
	{
		public UserInsufficientPermissionsException(string message) : base(message)
		{
		}
	}
}

