using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api[controller]")]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("order/{orderId}")]
        public async Task<IActionResult> GetByOrderId(int orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            try
            {
                var payment = await _paymentService.GetByOrderIdAsync(int.Parse(userId), orderId);

                if (payment == null)
                    return NotFound();

                return Ok(payment);
            }
            catch
            {
                return BadRequest("Something Went Wrong");
            }

           
        }
    }
}
