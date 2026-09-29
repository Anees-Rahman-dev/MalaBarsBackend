using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MalaBars.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService (
            ICartRepository cartRepository, 
            IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<List<CartItemDto>> GetCartAsync(int UserId)
        {
            var cartItem = await _cartRepository.GetByUserIdAsync(UserId);

            return cartItem.Select(item => new CartItemDto
            {
                CartItemId = item.CartItemId,
                ProductId = item.ProductId,
                ProductName = item.Product.Name,
                Price = item.Product.Price,
                Image = item.Product.Image,
                Quantity = item.Quantity,
                Total = item.Product.Price * item.Quantity
            }).ToList();
        }

        public async Task<CartItemDto> AddToCartAsync(int userId,AddToCartDto request)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            
                return null;

            if (request.Quantity <= 0)
                return null;

            if (request.Quantity > product.Stock)
                return null;

            var existingItem = await _cartRepository.GetByUserAndProductAsync(userId, request.ProductId);

            if (existingItem != null)
            {
                var newQuantity = existingItem.Quantity + request.Quantity;//when adding addtocart again and again ona a existing item.

                if (newQuantity > product.Stock)
                    return null;

                existingItem.Quantity = newQuantity; //

                    await _cartRepository.UpdateAsync(existingItem);

                return new CartItemDto
                {
                    CartItemId = existingItem.CartItemId,
                    ProductId = product.ProductId,
                    ProductName = product.Name,
                    Price = product.Price,
                    Image = product.Image,
                    Quantity = existingItem.Quantity,
                    Total = product.Price * existingItem.Quantity
                };
                
            }

            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };

            var createdItem = await _cartRepository.AddAsync(cartItem);

            return new CartItemDto
            {
                CartItemId = createdItem.CartItemId,
                ProductId = product.ProductId,
                ProductName = product.Name,
                Price = product.Price,
                Image = product.Image,
                Quantity = createdItem.Quantity,
                Total = product.Price * createdItem.Quantity
            };            
        }

        public async Task<bool> UpdateQuantityAsync(int userId,int cartItemId, int quantity)
        {
            if (quantity <= 0)
                return false;

            var cartItems = await _cartRepository.GetByUserIdAsync(userId);

            var cartItem = cartItems.FirstOrDefault(c => c.CartItemId == cartItemId);

            if (cartItem == null)
                return false;

            if (quantity > cartItem.Product.Stock)
                return false;

            cartItem.Quantity = quantity;

            return await _cartRepository.UpdateAsync(cartItem);
        }

        public async Task<bool> RemoveAsync(int userId, int cartItemId)
        {
            var cartItems = await _cartRepository.GetByUserIdAsync(userId);

            var cartItem = cartItems.FirstOrDefault(c => c.CartItemId == cartItemId);

            if (cartItem == null)
                return false;

            return await _cartRepository.DeleteAsync(cartItemId);

        }

    }
}
