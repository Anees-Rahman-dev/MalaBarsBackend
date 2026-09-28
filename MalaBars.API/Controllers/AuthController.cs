using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MalaBars.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthController (IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto request)
        {
            var user = await _userService.RegisterAsync(request);
            return Ok(user);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto request)
        {
            var token = await _userService.LoginAsync(request);
            if (token == null)
            {
                return Unauthorized("Invalid email or password Or Might Be Blocked By Admin.");
            }
            else
            {
                return Ok(new{token});
            }
        }
    }
}
