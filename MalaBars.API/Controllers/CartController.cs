using Azure.Core.Pipeline;
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
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);//So the user can only work with their own wishlist.

            if (userId == null)
                return Unauthorized();

            var cart = await _cartService.GetCartAsync(int.Parse(userId));

            return Ok(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(AddToCartDto request)
        {
            var user = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (user == null)
                return Unauthorized();

            var cartItem = await _cartService.AddToCartAsync(int.Parse(user), request);

            if (cartItem == null)
                return BadRequest("Invalid product, quantity, or stock.");

            return Ok(cartItem);
        }

        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, [FromBody] UpdateCartQuantityDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            if (userId == null)
                return Unauthorized();

            var updated = await _cartService.UpdateQuantityAsync(int.Parse(userId),cartItemId,request);

            if (!updated)
                return BadRequest("Invalid cart item or quantity.");

            return Ok("Cart quantity updated.");
        }

        [HttpDelete("{cartItemId}")]

        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var removed = await _cartService.RemoveAsync(int.Parse(userId),cartItemId);

            if (!removed)
                return NotFound("Cart item not found.");

            return NoContent();
        }

    }
}
