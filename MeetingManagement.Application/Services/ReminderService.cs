using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MeetingManagement.Application.Services
{
	public class ReminderService : BackgroundService
    {
        private readonly ILogger<ReminderService> _logger;

        public ReminderService(ILogger<ReminderService> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(10000, stoppingToken);

                await SomeRecurringTask();
            }
        }

        private Task SomeRecurringTask()
        {
            if (true)
            {
                Console.WriteLine("Executing background task");
                _logger.LogInformation("Logging in background task");
            }

            return Task.FromResult("Done");
        }
    }
}

