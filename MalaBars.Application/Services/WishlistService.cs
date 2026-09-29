using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;
        private readonly IProductRepository _productRepository;

        public WishlistService(IWishlistRepository wishlistRepository, IProductRepository productRepository)
        {
            _wishlistRepository = wishlistRepository;
            _productRepository = productRepository;
        }

        public async Task<List<WishlistItemDto>> GetWishlistAsync(int userId)
        {
            var items = await _wishlistRepository.GetByUserIdAsync(userId);

            return items.Select(item => new WishlistItemDto
            {
                WishlistItemId = item.WishlistItemId,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Image = item.Product.Image
            }).ToList();
        }

        public async Task<WishlistItemDto?> AddToWishlistAsync(int userId, int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);

            if (product == null)
                return null;

            var existing = await _wishlistRepository.GetByUserAndProductAsync(userId,productId);

            if (existing != null)
                return null;

            var wishListItem = new WishlistItem
            {
                UserId = userId,
                ProductId = productId
            };

            var createdItem = await _wishlistRepository.AddAsync(wishListItem);

            return new WishlistItemDto
            {
                WishlistItemId = createdItem.WishlistItemId,
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Image = product.Image
            };
        }

        public async Task<bool> RemoveFromWishlistAsync(int userId, int wishlistItemId)
        {
            var items = await _wishlistRepository.GetByUserIdAsync(userId);

            var item = items.FirstOrDefault(F => F.WishlistItemId == wishlistItemId);

            if (item == null)
                return false;

            return await _wishlistRepository.DeleteAsync(wishlistItemId);

        }
    }

}
