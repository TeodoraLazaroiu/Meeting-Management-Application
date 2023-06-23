using MeetingManagement.Application.DTOs.Mail;

namespace MeetingManagement.Application.Interfaces
{
	public interface IMailService
	{
        Task SendEmailAsync(SendMailDTO mailRequest);
    }
}

