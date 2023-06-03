﻿using System;
using MailKit.Net.Smtp;
using MailKit.Security;
using MeetingManagement.Application.DTOs.Mail;
using MeetingManagement.Application.Interfaces;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MeetingManagement.Application.Services
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail);
            email.To.Add(MailboxAddress.Parse(mailRequest.Recipient));
            email.Subject = mailRequest.Subject;

            var messageBuilder = new BodyBuilder();
            messageBuilder.HtmlBody = mailRequest.Message;
            email.Body = messageBuilder.ToMessageBody();

            using var smtp = new SmtpClient();
            smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
            await smtp.SendAsync(email);

            smtp.Disconnect(true);
        }
    }
}

