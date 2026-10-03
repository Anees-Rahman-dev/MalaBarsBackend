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
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var order = await _orderService.CreateOrderAsync(int.Parse(userId),request);

            if(order == null)
                return BadRequest("Cart is empty or product stock is insufficient.");

            return Ok(order);
        }

        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var myOrder = await _orderService.GetMyOrdersAsync(int.Parse(userId));

            return Ok(myOrder);
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var orderById = await _orderService.GetOrderByIdAsync(int.Parse(userId), orderId);

            if(orderById == null)
                return NotFound("Order not found.");

            return Ok(orderById);
        }

        [HttpGet("All")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return Ok(orders);
        }

        [HttpPut("{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] string status)
        {
            var updated = await _orderService.UpdateStatusAsync(orderId,status);

            if(!updated)
                return BadRequest("Invalid order status or order not found.");

            return Ok("Order status updated.");
        }

        [HttpGet("admin/{orderId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAdminOrderById(int orderId)
        {
            var order = await _orderService.GetAdminOrderByIdAsync(orderId);

            if(order == null)
                return NotFound("Order not found.");

            return Ok(order);
        }
    }



}
