using System.Security.Claims;
using MeetingManagement.Application.DTOs;
using MeetingManagement.Application.DTOs.Response;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/response")]
    [ApiController]
    [Authorize]
    public class ResponseController : ControllerBase
    {
        private readonly IResponseService _responseService;

        public ResponseController(IResponseService responseService)
        {
            _responseService = responseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserResponses()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var responses = await _responseService.GetUserResponses(userId);
                return Ok(responses);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutResponse([FromBody] UserResponseDTO userResponse)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _responseService.UpdateResponse(userId, userResponse);
                return NoContent();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

