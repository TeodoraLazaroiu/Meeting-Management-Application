using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/[controller]")]
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
        [AllowAnonymous]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetUserList();
                return Ok(users);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(string id)
        {
            try
            {
                var user = await _userService.GetUserEntity(id);
                return Ok(user);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [AllowAnonymous]
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(string id, [FromBody] UpdateUserDTO user)
        {
            try
            {
                await _userService.UpdateUser(id, user);
                return NoContent();

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpDelete("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
