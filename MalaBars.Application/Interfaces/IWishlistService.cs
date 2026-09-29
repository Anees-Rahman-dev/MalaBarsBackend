using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<List<WishlistItemDto>> GetWishlistAsync(int userId);

        Task<WishlistItemDto?> AddToWishlistAsync(int userId, int productId);

        Task<bool> RemoveFromWishlistAsync(int userId, int wishListItemId);


    }
}
