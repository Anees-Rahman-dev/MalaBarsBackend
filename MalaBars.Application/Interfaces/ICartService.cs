using MalaBars.Application.DTO_s;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface ICartService
    {
        Task<List<CartItemDto>> GetCartAsync(int id);
        Task<CartItemDto?> AddToCartAsync(int userId,AddToCartDto request);
        Task<bool> UpdateQuantityAsync(int id,int userId,UpdateCartQuantityDto request);
        Task<bool> RemoveAsync(int userId, int cartItemId);
    }
}
