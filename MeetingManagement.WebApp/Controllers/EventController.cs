using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/event")]
    [ApiController]
    [Authorize]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(string id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var eventDetails = await _eventService.GetEventById(id);
                return Ok(eventDetails);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserEvents(int year = 0, int month = 0, int day = 0)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var events = await _eventService.GetEventsForUser(userId, year, month, day);
                return Ok(events);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("team")]
        public async Task<IActionResult> GetTeamEvents(int year = 0, int month = 0, int day = 0)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var events = await _eventService.GetEventsForTeam(userId, year, month, day);
                return Ok(events);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("intervals")]
        public async Task<IActionResult> GenerateEventIntervals([FromBody] EventPlanningDTO eventPlan)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var intervals = await _eventService.GenerateEventIntervals(userId, eventPlan);
                return Ok(intervals);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> PostEvent([FromBody] CreateEventDTO eventDetails)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _eventService.CreateEvent(userId, eventDetails);
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEvent(string eventId)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _eventService.DeleteEvent(userId, eventId);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
