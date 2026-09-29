using MalaBars.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public async Task<IActionResult> GetWishList()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); //So the user can only work with their own wishlist.

            if (userId == null)
            {
                return Unauthorized();
            }
            else
            {
                var wishlist = await _wishlistService.GetWishlistAsync(int.Parse(userId));
                return Ok(wishlist);
            }
        }

        [HttpPost("{productId}")]

        public async Task<IActionResult> AddToWishlist(int productId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var item = await _wishlistService.AddToWishlistAsync(int.Parse(userId), productId);

            if (item == null)
                return BadRequest("Product does not exist or is already in wishlist.");

            return Ok(item);
        }

        [HttpDelete("{wishlistItemId}")]
        public async Task<IActionResult> RemoveFromWishlist(int wishlistItemId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var Removed = await _wishlistService.RemoveFromWishlistAsync(int.Parse(userId),wishlistItemId);

            if(!Removed)

                return NotFound("Wishlist item not found.");

                return NoContent();
        }

    }
}
