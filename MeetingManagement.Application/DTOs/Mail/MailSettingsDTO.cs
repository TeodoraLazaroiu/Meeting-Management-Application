using System;
namespace MeetingManagement.Application.DTOs.Mail
{
	public class MailSettingsDTO
	{
        public string Mail { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Host { get; set; } = null!;
        public int Port { get; set; }
    }
}

