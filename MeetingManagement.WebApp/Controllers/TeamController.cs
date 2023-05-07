﻿using MeetingManagement.Application.DTOs.Team;
using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Interfaces;
using MeetingManagement.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/team")]
    [ApiController]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;

        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        // todo: remove
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeam()
        {
            try
            {
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> PostCreateTeam([FromBody] CreateTeamDTO teamDetails)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var teamEntity = await _teamService.CreateTeam(userId, teamDetails);
                return CreatedAtAction(nameof(GetTeam), new {id = teamEntity.Id }, teamEntity.AccessCode);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("join")]
        [AllowAnonymous]
        public async Task<IActionResult> PostJoinTeam([FromBody] string accessCode)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _teamService.JoinTeam(userId, accessCode);
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeam([FromBody] UpdateUserDTO user)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                return NoContent();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteTeam()
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
    }
}
