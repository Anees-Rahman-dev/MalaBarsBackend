using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetByUserIdAsync(int id);

        Task<CartItem> GetByUserAndProductAsync(int id,int productId);

        Task<CartItem> AddAsync(CartItem cartItem);

        Task<bool> UpdateAsync(CartItem cartItem);
        
        Task<bool> DeleteAsync(int cartItemId);
    }
}
