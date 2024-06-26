﻿using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Application.Services;
using MeetingManagement.Core.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var user = await _userService.GetUserInfo(userId);
                return Ok(user);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> PostUser([FromBody] RegisterUserDTO user)
        {
            try
            {
                var userId = await _userService.RegisterUser(user);
                return CreatedAtAction(nameof(GetUser), new {id = userId }, userId);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUser([FromBody] UpdateUserDTO user)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _userService.UpdateUser(userId, user);
                return NoContent();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _userService.DeleteUser(userId);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
