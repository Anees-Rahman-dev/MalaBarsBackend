using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;
        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
    public async Task<List<CartItem>> GetByUserIdAsync(int userId)
        {
            return await _context.cartItems
            .Include(c => c.Product)//when we get the user's cart, EF Core also loads the Product information.
            .Where(c => c.UserId == userId)
            .ToListAsync();
        }

        public async Task<CartItem?> GetByUserAndProductAsync(
            int userId,int productId)
        {
            return await _context.cartItems
                .FirstOrDefaultAsync(c =>
                    c.UserId == userId &&
                    c.ProductId == productId);
        }

        public async Task<CartItem> AddAsync(CartItem cartItem)
        {
            _context.cartItems.Add(cartItem);

            await _context.SaveChangesAsync();

            return cartItem;
        }

        public async Task<bool> UpdateAsync(CartItem cartItem)
        {
            var existing = await _context.cartItems.FindAsync(cartItem.CartItemId);

            if (existing == null)
            {
                return false;
            }
            else
            {
                existing.Quantity = cartItem.Quantity;
             await _context.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> DeleteAsync(int cartItemId)
        {
            var Existing = await _context.cartItems.FindAsync(cartItemId);

            if(Existing == null)
            {
                return false;
            }
            else
            {
                _context.cartItems.Remove(Existing);
                await _context.SaveChangesAsync();
            }
            return true;

        }
             
    }
}
