using System;
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

        public EventService(IEventRepository eventRepository, IRecurringPatternRepository recurringPatternRepository)
        {
            _eventRepository = eventRepository;
            _recurringPatternRepository = recurringPatternRepository;
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
                recurringPattern.WeekOfMonth = eventDetails.WeekOfMonth;
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
	}
}

