using MeetingManagement.Application.DTOs.User;
using MeetingManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("/signIn")]
        public async Task<IActionResult> SignInUser([FromBody] SignInUserDTO userCredentials)
        {
            try
            {
                await _authService.SignInUser(userCredentials);
                return Ok("Signed in successfully");

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("/signOut")]
        public async Task<IActionResult> SignOutUser()
        {
            try
            {
                await _authService.SignOutUser();
                return Ok("Signed in successfully");

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
