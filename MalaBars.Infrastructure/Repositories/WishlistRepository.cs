using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly ApplicationDbContext _context;
        public WishlistRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<WishlistItem>> GetByUserIdAsync(int userId)
        {
            return await _context.wishlistItems
                .Include(W => W.Product)//so the frontend can receive product information along with the wishlist item.
                .Where(W => W.UserId == userId)
                .ToListAsync();
        }


        public async Task<WishlistItem?> GetByUserAndProductAsync(int userId, int productId)
        {
            return await _context.wishlistItems.FirstOrDefaultAsync(W => W.UserId == userId && W.ProductId == productId);
        }

        public async Task<WishlistItem> AddAsync(WishlistItem wishlist)
        {
            _context.wishlistItems.Add(wishlist);

            await _context.SaveChangesAsync();

            return wishlist;
        }

        public async Task<bool> DeleteAsync(int wishListId)
        {

            var Deleted = await _context.wishlistItems.FindAsync(wishListId);

            if(Deleted == null)
            {
                return false;
            }
            else
            {
                _context.wishlistItems.Remove(Deleted);

                await _context.SaveChangesAsync();
            }


            return true;
        }

    }
}
