using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepo;

        public UserController(IUserRepository userRepo)
        {
            _userRepo = userRepo;
        }

        [HttpGet("GetMe")]
        public async Task<IActionResult> GetMe()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var user = await _userRepo.GetByIdAsync(int.Parse(userId));

            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsBlocked = user.IsBlocked
            };

            return Ok(userDto);
        }



        [HttpGet]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> GetAllUser()
        {
            var users = await _userRepo.GetAllAsync();

            var userDtos = users.Select(user => new UserDto
            {
                UserId = user.UserId,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                IsBlocked = user.IsBlocked
            }).ToList();

            return Ok(userDtos);
        }

        [HttpPut("{id}/Block")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> BlockUser(int id)
        {
            var blocked = await _userRepo.BlockAsync(id);

            if (!blocked)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

        [HttpPut("{id}/UnBlock")]
        [Authorize]

        public async Task<IActionResult> UnBlockUser(int id)
        {
            var Unblocked = await _userRepo.UnblockAsync();
            if (Unblocked == null)
            {
                return NotFound();
            }
            else
            {
                return NoContent();
            }
        }

    }

}