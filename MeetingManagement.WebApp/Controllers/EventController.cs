using MeetingManagement.Application.DTOs.Event;
using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Application.DTOs.User;
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

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserEvents()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var events = await _eventService.GetEventsForUser(userId);
                return Ok(events);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("team")]
        public async Task<IActionResult> GetTeamEvents()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                return Ok();

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
    }
}
