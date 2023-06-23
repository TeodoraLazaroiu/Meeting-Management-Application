using MeetingManagement.Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MeetingManagement.Application.DTOs.Response;
using MeetingManagement.Application.DTOs.Mail;

namespace MeetingManagement.Application.Services
{
    public class ReminderService : BackgroundService
    {
        private readonly ILogger<ReminderService> _logger;
        public IServiceProvider services { get; }

        public ReminderService(ILogger<ReminderService> logger, IServiceProvider service)
        {
            _logger = logger;
            services = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                Console.WriteLine("Before starting background service: {0}", now);
                await CheckEventsAndReminders();

                now = DateTime.Now;
                Console.WriteLine("After running background service: {0}", now);

                var second = now.Second;
                await Task.Delay(60000 - second * 1000, stoppingToken);
            }
        }

        private async Task CheckEventsAndReminders()
        {
            try
            {
                _logger.LogInformation("Starting background task");
                using (var scope = services.CreateScope())
                {
                    var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
                    var responseService = scope.ServiceProvider.GetRequiredService<IResponseService>();
                    var emailService = scope.ServiceProvider.GetRequiredService<IMailService>();

                    var year = DateTime.Now.Year;
                    var month = DateTime.Now.Month;
                    var day = DateTime.Now.Day;
                    var todayEvents = await eventService.GetEvents(year, month, day);

                    var responses = new List<ResponseDetailsDTO>();

                    foreach(var eventOccurence in todayEvents)
                    {
                        var eventResponses = await responseService.GetResponsesByEvent(eventOccurence.Id);
                        responses.AddRange(eventResponses);
                    }

                    var reminderResponses = responses.Where(x => x.SendReminder == true).ToList();
                    foreach(var response in reminderResponses)
                    {
                        if (response.ReminderTime == null) continue;
                        var reminderTime = TimeSpan.Parse(response.StartTime).Subtract(TimeSpan.FromMinutes((double)response.ReminderTime));

                        var now = DateTime.Now;

                        var currentHour = now.Hour;
                        var currentMinute = now.Minute;

                        var reminderHour = reminderTime.Hours;
                        var reminderMinute = reminderTime.Minutes;

                        if (reminderHour == currentHour && reminderMinute == currentMinute)
                        {
                            _logger.LogInformation("Sending email to: {email}", response.UserEmail);
                            var request = new SendMailDTO();
                            request.Recipient = response.UserEmail ?? "";
                            request.Subject = $"Reminder for meeting: {response.EventTitle}";
                            request.Message = $"Your meeting will start in {response.ReminderTime} minutes";
                            await emailService.SendEmailAsync(request);
                        }
                    }
                    _logger.LogInformation("Ending background task");
                }
            }
            catch
            {
                
            }
        }
    }
}

