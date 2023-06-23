using System;
namespace MeetingManagement.Application.DTOs.Mail
{
    public class SendMailDTO
    {
        public string Recipient { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}

