using MeetingManagement.Application.DTOs;
using MeetingManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeetingManagement.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "test";
        }

        [HttpPost]
        public void Post([FromBody] RegisterUserDTO user)
        {
            try
            {

            }
            catch (Exception)
            {
                throw new Exception("The server encountered an unexpected error.");
            }
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
