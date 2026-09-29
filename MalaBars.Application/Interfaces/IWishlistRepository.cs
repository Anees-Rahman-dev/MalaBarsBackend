using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task<List<WishlistItem>> GetByUserIdAsync(int userId);

        Task<WishlistItem> GetByUserAndProductAsync(int userId, int productId);

        Task<WishlistItem> AddAsync(WishlistItem wishlistItem);

        Task<bool> DeleteAsync(int wishlistItemId);
    }
}
