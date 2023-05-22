using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Application.Exceptions;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Core.Common;
using MeetingManagement.Core.Entities;
using MeetingManagement.Core.Interfaces;

namespace MeetingManagement.Application.Services
{
	public class EventService : IEventService
	{
        private IEventRepository _eventRepository;
        private IRecurringPatternRepository _recurringPatternRepository;
        private ITeamService _teamService;

        public EventService(IEventRepository eventRepository, IRecurringPatternRepository
            recurringPatternRepository, ITeamService teamService)
        {
            _eventRepository = eventRepository;
            _recurringPatternRepository = recurringPatternRepository;
            _teamService = teamService;
        }

		public async Task CreateEvent(string userId, CreateEventDTO eventDetails)
        { 
            var eventEntity = new EventEntity();

            eventEntity.Id = Guid.NewGuid();
            eventEntity.CreatedDate = DateTime.UtcNow;
            eventEntity.LastModified = DateTime.UtcNow;
            eventEntity.EventTitle = eventDetails.EventTitle;
			eventEntity.EventDescription = eventDetails.EventDescription;
            eventEntity.Attendes = eventDetails.Attendes;
            eventEntity.Attendes.Add(new Guid(userId));

            try
            {
                var hourMinutes = eventDetails.StartTime.Split(":").Select(Int32.Parse).ToList();
                eventEntity.StartTime = new TimeOnly(hourMinutes[0], hourMinutes[1]).ToTimeSpan();

            }
            catch
            {
                throw new EventValidationException("Invalid start time format. Should be hh:mm");
            }

            try
            {
                var hourMinutes = eventDetails.EndTime.Split(":").Select(Int32.Parse).ToList();
                eventEntity.EndTime = new TimeOnly(hourMinutes[0], hourMinutes[1]).ToTimeSpan();
            }
            catch
            {
                throw new EventValidationException("Invalid end time format. Should be hh:mm");
            }

            try
            {
                var yearMonthDay = eventDetails.StartDate.Split("/").Select(Int32.Parse).ToList();
                var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                eventEntity.StartDate = dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            catch
            {
                throw new EventValidationException("Invalid start date format. Should be dd/mm/yyyy");
            }

            try
            {
                var yearMonthDay = eventDetails.EndDate.Split("/").Select(Int32.Parse).ToList();
                var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                eventEntity.EndDate = dateOnly.ToDateTime(TimeOnly.MinValue);
            }
            catch
            {
                throw new EventValidationException("Invalid end date format. Should be dd/mm/yyyy");
            }

            eventEntity.IsRecurring = eventDetails.IsRecurring;
            eventEntity.CreatedBy = new Guid(userId);

            if (eventEntity.StartDate > eventEntity.EndDate || eventEntity.StartDate < DateTime.Now)
            {
                throw new EventValidationException("Invalid timeline for the date of event");
            }

            if (eventEntity.StartTime > eventEntity.EndTime)
            {
                throw new EventValidationException("Invalid timeline for the time of event");
            }

            if (eventEntity.IsRecurring)
			{
                var recurrenceTypes = Enum.GetValues(typeof(ReccurenceType)).Cast<ReccurenceType>().ToList();
                if (!recurrenceTypes.Contains(eventDetails.ReccurenceType))
                {
                    throw new EventValidationException("Invalid recurrence type");
                }

				var recurringPattern = new RecurringPatternEntity();

                recurringPattern.Id = eventEntity.Id;
				recurringPattern.ReccurenceType = eventDetails.ReccurenceType;
                recurringPattern.SeparationCount = eventDetails.SeparationCount;
                recurringPattern.NumberOfOccurences = eventDetails.NumberOfOccurences;
                recurringPattern.DaysOfWeek = eventDetails.DaysOfWeek;
                recurringPattern.DayOfWeek = eventDetails.DayOfWeek;
                recurringPattern.DayOfMonth = eventDetails.DayOfMonth;

                await _recurringPatternRepository.CreateAsync(recurringPattern);
            }

            await _eventRepository.CreateAsync(eventEntity);
        }

        public async Task<List<EventEntity>> GetEventsForUser(string userId)
        {
            var events = await _eventRepository.GetEventsByUserId(userId);
            return events;
        }

        public async Task<List<EventEntity>> GetEventsForTeam(string userId)
        {
            var teamDetails = await _teamService.GetTeamByUserId(userId);
            var userIds = teamDetails.TeamMembers.Select(x => x.Id).ToList();
            var events = await _eventRepository.GetEventsForUserIds(userIds);
            return events;
        }

        public async Task<List<EventIntervalsDTO>> GenerateEventIntervals(string userId, EventPlanningDTO eventPlan)
        {
            var users = new List<Guid>();
            users.AddRange(eventPlan.Attendes);
            users.Add(new Guid(userId));

            // add validation for input

            var events = await _eventRepository.GetEventsForUserIds(users);
            var eventsForToday = events.Where(x => x.StartDate <= DateTime.UtcNow && x.EndDate >= DateTime.UtcNow).ToList();

            // to do: get working hours for team
            var workingHours = (8, 18);
            var intervals = new List<(int, int)>();

            for (int i = workingHours.Item1; i < workingHours.Item2; i++)
            {
                intervals.Add((i, i + 1));
            }

            foreach (var eventEntry in eventsForToday)
            {
                int start = 0, end = 0;
                if (eventEntry.IsRecurring)
                {
                    var recurrence = await _recurringPatternRepository.GetAsync(eventEntry.Id.ToString())
                        ?? throw new EventNotFoundException();

                    if (recurrence.ReccurenceType == ReccurenceType.Daily)
                    {
                        if (recurrence.DaysOfWeek == null || !recurrence.DaysOfWeek.Any())
                        {
                            start = eventEntry.StartTime.Hours;
                            end = eventEntry.EndTime.Hours;
                        }
                        else
                        {
                            var yearMonthDay = eventPlan.StartDate.Split("/").Select(Int32.Parse).ToList();
                            var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                            var day = (int)dateOnly.DayOfWeek;

                            if (day == 0) day = 7;

                            if (recurrence.DaysOfWeek.Contains(day))
                            {
                                start = eventEntry.StartTime.Hours;
                                end = eventEntry.EndTime.Hours;
                            }
                        }
                    }
                    else if (recurrence.ReccurenceType == ReccurenceType.Weekly)
                    {
                        var yearMonthDay = eventPlan.StartDate.Split("/").Select(Int32.Parse).ToList();
                        var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                        var day = (int)dateOnly.DayOfWeek;

                        if (day == 0) day = 7;

                        if (recurrence.SeparationCount == 0 || recurrence.SeparationCount == null)
                        {
                            if (recurrence.DayOfWeek == day)
                            {
                                start = eventEntry.StartTime.Hours;
                                end = eventEntry.EndTime.Hours;
                            }
                        }
                        else
                        {
                            int separation = (int)recurrence.SeparationCount;
                            var startDate = DateOnly.FromDateTime(eventEntry.StartDate).AddDays(day - 1);
                            var endDate = DateOnly.FromDateTime(eventEntry.EndDate);

                            while (startDate < endDate)
                            {
                                if (startDate == dateOnly)
                                {
                                    start = eventEntry.StartTime.Hours;
                                    end = eventEntry.EndTime.Hours;
                                }
                                startDate.AddDays(7 * (separation + 1));
                            }
                        }
                    }
                    else
                    {
                        var yearMonthDay = eventPlan.StartDate.Split("/").Select(Int32.Parse).ToList();
                        var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                        var day = dateOnly.Day;

                        if (recurrence.SeparationCount == 0 || recurrence.SeparationCount == null)
                        {
                            if (recurrence.DayOfMonth == day)
                            {
                                start = eventEntry.StartTime.Hours;
                                end = eventEntry.EndTime.Hours;
                            }
                        }
                        else
                        {
                            int separation = (int)recurrence.SeparationCount;
                            var startDate = DateOnly.FromDateTime(eventEntry.StartDate).AddDays(day - 1);
                            var endDate = DateOnly.FromDateTime(eventEntry.EndDate);

                            while (startDate < endDate)
                            {
                                if (startDate == dateOnly)
                                {
                                    start = eventEntry.StartTime.Hours;
                                    end = eventEntry.EndTime.Hours;
                                }
                                startDate.AddMonths(separation + 1);
                            }
                        }
                    }

                }
                else
                {
                    start = eventEntry.StartTime.Hours;
                    end = eventEntry.EndTime.Hours;
                }

                for (int i = start; i < end; i++)
                {
                    if (intervals.Contains((i, i + 1)))
                    {
                        intervals.Remove((i, i + 1));
                    }
                }
            }

            var rnd = new Random();
            intervals = intervals.OrderBy(x => rnd.Next()).Take(3).ToList();
            return intervals.Select(x => new EventIntervalsDTO(x.Item1, x.Item2)).ToList();
        }
    }
}

