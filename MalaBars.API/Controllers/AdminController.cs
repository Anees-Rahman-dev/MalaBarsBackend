using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api/AdminDashboard")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        //Dashboard 

        [HttpGet("allStats")]
        public async Task<IActionResult> GetStats()
        {
            return Ok(await _adminService.GetDashboardStatsAsync());
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _adminService.GetAllUsersAsync());
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _adminService.GetUserByIdAsync(id);
            return user == null ? NotFound() : Ok(user); 
        }

        [HttpPut("users/{id}/block")]
        public async Task<IActionResult> ToggleBlock(int id)
        {
            var result = await _adminService.ToggleBlockUserAsync(id);
            return result ? NoContent() : NotFound();
        }

        [HttpPut("users/{id}/role")]
        public async Task<IActionResult> ChangeRole(int id, ChangeRoleDto dto)
        {
            try
            {
                var result = await _adminService.ChangeUserRoleAsync(id, dto);
                return result ? NoContent() : NotFound();
            }
            catch(Exception exe)
            {
                return BadRequest(new { message = exe.Message });
            }
        }

        [HttpDelete("users/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _adminService.DeleteUserAsync(id);
            return user ? NoContent() : NotFound();
        }

        [HttpGet("users/{id}/orders")]
        public async Task<IActionResult> GetUserOrders(int id)
        {
            return Ok(await _adminService.GetUserOrdersAsync(id));
        }

        //Order management

        [HttpGet("orders")]
        public async Task<IActionResult> GetAllOrder()
        {
            return Ok(await _adminService.GetAllOrdersAsync());
        }

        [HttpGet("orders/{id}")]
        public async Task<IActionResult?> GetOrder(int id)
        {
            var order = await _adminService.GetOrderByIdAsync(id);
            return order ==  null ? NotFound() : Ok(order);
        }

        [HttpPut("orders/{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, UpdateOrderStatusDto dto)
        {
            try
            {
                var res = await _adminService.UpdateOrderStatusAsync(id, dto);
                return res ? NoContent() : NotFound(); 
            }
            catch(Exception exe)
            {
                return BadRequest(new { message = exe.Message });
            }
        }

    }
}
