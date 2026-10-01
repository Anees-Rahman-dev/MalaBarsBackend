using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MalaBars.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyAddress()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return Unauthorized();
            }else
            {
                var address = await _addressService.GetMyAddressesAsync(int.Parse(userId));
                return Ok(address);
            }
        }

        [HttpGet("{addressId}")]

        public async Task<IActionResult> GetAddressById( int addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if(userId == null)
            {
                return Unauthorized();
            }
            else
            {
                var address = await _addressService.GetByIdAsync(int.Parse(userId), addressId);

                return address == null ? NotFound() : Ok(address);
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(AddressDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            else
            {
                var createdAddress = await _addressService.AddAsync(int.Parse(userId),request);

                return Ok(createdAddress);
            }
        }

        [HttpPut("{addressId}")]
        public async Task<IActionResult> UpdateAddress(int addressId, AddressDto request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            else
            {
                var updatedAddress = await _addressService.UpdateAsync(int.Parse(userId),addressId,request);

                return !updatedAddress ? NotFound("Cannot Find") : Ok("Address updated succesfully"); 
            }

            
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddress(int addressId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
            return Unauthorized();
            }

            try
            {
                var deletedAddress = await _addressService.DeleteAsync(int.Parse(userId),addressId);

                        if (!deletedAddress)
                        return NotFound("Cannot Find");
 
                        return Ok(deletedAddress);
            }
            catch
            {
                return BadRequest("Something Went Wrong");
            }
        }
    }
}
