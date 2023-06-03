using System.ComponentModel;
using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Application.DTOs.User;
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
        private IResponseRepository _responseRepository;
        private ITeamService _teamService;
        private IUserService _userService;
        private IResponseService _responseService;

        public EventService(IEventRepository eventRepository, IRecurringPatternRepository recurringPatternRepository,
            IResponseRepository responseRepository, ITeamService teamService, IUserService userService, IResponseService responseService)
        {
            _eventRepository = eventRepository;
            _recurringPatternRepository = recurringPatternRepository;
            _responseRepository = responseRepository;
            _teamService = teamService;
            _userService = userService;
            _responseService = responseService;
        }

		public async Task CreateEvent(string userId, CreateEventDTO eventDetails)
        { 
            var eventEntity = new EventEntity();

            eventEntity.Id = Guid.NewGuid();
            eventEntity.CreatedDate = DateTime.UtcNow;
            eventEntity.LastModified = DateTime.UtcNow;
            eventEntity.EventTitle = eventDetails.EventTitle;
			eventEntity.EventDescription = eventDetails.EventDescription;
            eventEntity.Attendes = eventDetails.Attendes.Select(x => new Guid(x)).ToList();
            if (!eventDetails.Attendes.Contains(userId)) eventEntity.Attendes.Add(new Guid(userId));

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

            // datetimes are saved with -3 hours: 1.06.2023 00:00 -> 31.05.2023 21:00
            try
            {
                var yearMonthDay = eventDetails.StartDate.Split("/").Select(Int32.Parse).ToList();
                var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                eventEntity.StartDate = dateOnly.ToDateTime(TimeOnly.MaxValue);
            }
            catch
            {
                throw new EventValidationException("Invalid start date format. Should be dd/mm/yyyy");
            }

            try
            {
                var yearMonthDay = eventDetails.EndDate.Split("/").Select(Int32.Parse).ToList();
                var dateOnly = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
                eventEntity.EndDate = dateOnly.ToDateTime(TimeOnly.MaxValue);
            }
            catch
            {
                throw new EventValidationException("Invalid end date format. Should be dd/mm/yyyy");
            }

            eventEntity.IsRecurring = eventDetails.IsRecurring;
            eventEntity.CreatedBy = new Guid(userId);

            if (eventEntity.StartDate > eventEntity.EndDate)
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

            foreach (var attendee in eventEntity.Attendes)
            {
                var response = new ResponseEntity();
                response.EventId = eventEntity.Id;
                response.UserId = attendee;
                await _responseRepository.CreateAsync(response);
            }
        }

        private async Task<EventEntity> GetEventEntity(string id)
        {
            try
            {
                var eventEntity = await _eventRepository.GetAsync(id)
                    ?? throw new EventNotFoundException();
                return eventEntity;
            }
            catch
            {
                throw new EventNotFoundException();
            }
        }

        public async Task<EventDetailsDTO> GetEventById(string id)
        {
            var eventEntity = await GetEventEntity(id);
            var eventDetails = new EventDetailsDTO();

            eventDetails.EventTitle = eventEntity.EventTitle;
            eventDetails.EventDescription = eventEntity.EventDescription;
            eventDetails.StartTime = eventEntity.StartTime.ToString("hh\\:mm");
            eventDetails.EndTime = eventEntity.EndTime.ToString("hh\\:mm");
            eventDetails.StartDate = eventEntity.StartDate.Date.ToString("dddd, dd MMMM yyyy");
            eventDetails.EndDate = eventEntity.EndDate.Date.ToString("dddd, dd MMMM yyyy");
            eventDetails.IsRecurring = eventEntity.IsRecurring;

            var attendees = new List<UserInfoDTO>();
            foreach (var userId in eventEntity.Attendes)
            {
                var userInfo = await _userService.GetUserInfo(userId.ToString());
                attendees.Add(userInfo);
            }
            eventDetails.AttendesInfo = attendees;

            eventDetails.CreatedBy = await _userService.GetUserInfo(eventEntity.CreatedBy.ToString());

            return eventDetails;
        }

        public async Task<List<EventOccurenceDTO>> GetEvents(int year = 0, int month = 0, int day = 0)
        {
            var eventsFromDb = (await _eventRepository.GetAllAsync()).ToList();
            var eventOccurences = await GetEventsOccurences(eventsFromDb, year, month, day);
            return eventOccurences;
        }

        public async Task<List<EventOccurenceDTO>> GetEventsForUser(string userId, int year = 0, int month = 0, int day = 0)
        {
            var eventsFromDb = await _eventRepository.GetEventsByUserId(userId);
            var eventOccurences = await GetEventsOccurences(eventsFromDb, year, month, day);
            return eventOccurences;
        }

        public async Task<List<EventOccurenceDTO>> GetEventsForTeam(string userId, int year = 0, int month = 0, int day = 0)
        {
            var teamDetails = await _teamService.GetTeamByUserId(userId);
            var userIds = teamDetails.TeamMembers.Select(x => x.Id).ToList();
            var events = await _eventRepository.GetEventsForUserIds(userIds);

            var eventOccurences = await GetEventsOccurences(events, year, month, day);
            return eventOccurences;
        }

        public async Task DeleteEvent(string userId, string eventId)
        {
            var eventEntity = await GetEventEntity(eventId);

            if (eventEntity.CreatedBy.ToString() != userId)
            {
                throw new EventDeletionException("You do not have rights to delete this event");
            }

            await _eventRepository.DeleteAsync(eventId);
            await _responseRepository.DeleteResponsesByEvent(eventId);
            if (eventEntity.IsRecurring) { await _recurringPatternRepository.DeleteAsync(eventId); }
        }

        public async Task<List<EventIntervalsDTO>> GenerateEventIntervals(string userId, EventPlanningDTO eventPlan)
        {
            var users = new List<Guid>();
            users.AddRange(eventPlan.Attendes);
            users.Add(new Guid(userId));

            DateOnly selectedDate = new DateOnly();
            try
            {
                var yearMonthDay = eventPlan.Date.Split("/").Select(Int32.Parse).ToList();
                selectedDate = new DateOnly(yearMonthDay[2], yearMonthDay[1], yearMonthDay[0]);
            }
            catch
            {
                throw new EventValidationException("Invalid date format. Should be dd/mm/yyyy");
            }

            var events = await _eventRepository.GetEventsForUserIds(users);
            var eventsForDate = events.Where(x => DateOnly.FromDateTime(x.StartDate)
                <= selectedDate && DateOnly.FromDateTime(x.EndDate) >= selectedDate).ToList();
            var eventOccurencesForDate = await GetEventsOccurences(eventsForDate,
                    selectedDate.Year, selectedDate.Month, selectedDate.Day);

            var teamDetails = await _teamService.GetTeamByUserId(userId);
            var start = teamDetails.StartWorkingHour;
            var end = teamDetails.EndWorkingHour;
            var intervals = new List<(int, int)>();

            for (int i = start; i < end; i++)
            {
                intervals.Add((i, i + 1));
            }

            foreach (var eventOccurence in eventOccurencesForDate)
            {
                int startHour, endHour;
                var hourMinutes = eventOccurence.EndTime.Split(":").Select(Int32.Parse).ToList();
                var startTime = new TimeOnly(hourMinutes[0], hourMinutes[1]);
                startHour = startTime.Hour;

                hourMinutes = eventOccurence.EndTime.Split(":").Select(Int32.Parse).ToList();
                var endTime = new TimeOnly(hourMinutes[0], hourMinutes[1]);

                if (endTime.Minute != 0) endHour = endTime.Hour + 1;
                else endHour = endTime.Hour;

                for (int i = startHour; i < endHour; i++)
                {
                    if (intervals.Contains((i, i + 1)))
                    {
                        intervals.Remove((i, i + 1));
                    }
                }
            }

            var rnd = new Random();
            intervals = intervals.OrderBy(x => rnd.Next()).Take(3).OrderBy(x => x.Item1).ToList();
            return intervals.Select(x => new EventIntervalsDTO(x.Item1, x.Item2)).ToList();
        }

        private async Task<List<EventOccurenceDTO>> GetEventsOccurences(List<EventEntity> events, int year = 0, int month = 0, int day = 0)
        {
            var eventsOccurences = new List<EventOccurenceDTO>();

            foreach (var eventEntry in events)
            {
                if (eventEntry.IsRecurring)
                {
                    var recurrence = await _recurringPatternRepository.GetAsync(eventEntry.Id.ToString())
                        ?? throw new EventNotFoundException();

                    var currentDate = eventEntry.StartDate;
                    var endDate = eventEntry.EndDate;

                    while (currentDate <= endDate)
                    {
                        if (recurrence.ReccurenceType == ReccurenceType.Daily)
                        {
                            if (recurrence.DaysOfWeek == null)
                            {
                                throw new EventValidationException("Daily recurrence must provide DaysOfWeek parameter");
                            }

                            var daysOfWeek = Enumerable.Range(1, 7).ToList();
                            if (recurrence.DaysOfWeek == daysOfWeek)
                            {
                                eventsOccurences.Add(new EventOccurenceDTO(eventEntry, currentDate));
                            }
                            else
                            {
                                var currentDayOfWeek = (int)currentDate.DayOfWeek;
                                if (currentDayOfWeek == 0) currentDayOfWeek = 7;

                                if (recurrence.DaysOfWeek.Contains(currentDayOfWeek))
                                {
                                    eventsOccurences.Add(new EventOccurenceDTO(eventEntry, currentDate));
                                }
                            }
                            currentDate = currentDate.AddDays(1);
                        }
                        else if (recurrence.ReccurenceType == ReccurenceType.Weekly)
                        {
                            if (recurrence.DayOfWeek == null)
                            {
                                throw new EventValidationException("Weekly recurrence must provide DayOfWeek parameter");
                            }

                            var currentDayOfWeek = (int)currentDate.DayOfWeek;
                            if (currentDayOfWeek == 0) currentDayOfWeek = 7;

                            if (recurrence.DayOfWeek != currentDayOfWeek)
                            {
                                if (currentDayOfWeek < recurrence.DayOfWeek)
                                {
                                    var offset = recurrence.DayOfWeek - currentDayOfWeek;
                                    currentDate = currentDate.AddDays((int)offset);
                                }
                                else
                                {
                                    var offset = 7 - (currentDayOfWeek - recurrence.DayOfWeek);
                                    currentDate = currentDate.AddDays((int)offset);
                                }
                            }
                            else
                            {
                                eventsOccurences.Add(new EventOccurenceDTO(eventEntry, currentDate));
                                var separation = recurrence.SeparationCount ?? throw new
                                    EventValidationException("Weekly recurrence must provide SeparationCount parameter");
                                currentDate = currentDate.AddDays(7 * (separation + 1));
                            }
                        }
                        else
                        {
                            if (recurrence.DayOfMonth == null)
                            {
                                throw new EventValidationException("Monthly recurrence must provide DayOfMonth parameter");
                            }

                            var daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
                            var lastDays = new List<int>() { 29, 30, 31 };

                            if (currentDate.Day != recurrence.DayOfMonth)
                            {
                                var targetDay = recurrence.DayOfMonth;
                                if (recurrence.DayOfMonth > daysInMonth)
                                {
                                    targetDay = daysInMonth;
                                }

                                while (currentDate.Day != targetDay)
                                {
                                    currentDate = currentDate.AddDays(1);
                                }
                            }

                            eventsOccurences.Add(new EventOccurenceDTO(eventEntry, currentDate));
                            var separation = recurrence.SeparationCount ?? throw new
                                EventValidationException("Monthly recurrence must provide SeparationCount parameter");
                            currentDate = currentDate.AddMonths(separation + 1);
                        }
                    }
                }
                else
                {
                    eventsOccurences.Add(new EventOccurenceDTO(eventEntry, eventEntry.StartDate));
                }
            }

            if (year != 0) eventsOccurences = eventsOccurences.Where(x => DateTime.Parse(x.Date).Year == year).ToList();
            if (month != 0) eventsOccurences = eventsOccurences.Where(x => DateTime.Parse(x.Date).Month == month).ToList();
            if (day != 0) eventsOccurences = eventsOccurences.Where(x => DateTime.Parse(x.Date).Day == day).ToList();

            return eventsOccurences;
        }
    }
}

