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

        // to do: remove
        [HttpGet("all")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllTeams()
        {
            try
            {
                var teams = await _teamService.GetAllTeams();
                return Ok(teams);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTeam()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                var team = await _teamService.GetTeamByUserId(userId);
                return Ok(team);

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
                await _teamService.JoinTeam(userId, teamEntity.AccessCode);
                return CreatedAtAction(nameof(GetTeam), new {id = teamEntity.Id }, teamEntity.AccessCode);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("join")]
        [AllowAnonymous]
        public async Task<IActionResult> PostJoinTeam(string accessCode)
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

        // to do: add check for admin
        [HttpDelete]
        public async Task<IActionResult> DeleteTeam()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimConstants.UserIdClaim);
                await _teamService.DeleteTeam(userId);
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
