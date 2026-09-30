using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto?> CreateOrderAsync(int userId)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {
                var cartItems = await _cartRepository.GetByUserIdAsync(userId);

                if (!cartItems.Any())

                    return null;


                decimal totalAmount = 0;

                var order = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending"
                };

                foreach (var cartItem in cartItems) // getting the product and
                                                    // calculating total, creating order, reducing the stock, updating the product EndPoint.
                {
                    var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

                    if (product == null)
                        throw new Exception("Product not found.");

                    if (cartItem.Quantity > product.Stock)
                        throw new Exception("Insufficient Stock.");


                    var ItemTotal = product.Price * cartItem.Quantity;

                    totalAmount += ItemTotal;

                    var orderItem = new OrderItem
                    {
                        ProductId = product.ProductId,
                        Quantity = cartItem.Quantity,
                        Price = product.Price
                    };

                    order.OrderItems.Add(orderItem);

                    product.Stock -= cartItem.Quantity;

                    await _productRepository.UpdateAsync(product);//letting the product know that this particular product's this much quantity has reduced
                }

                order.TotalAmount = totalAmount;

                var orderCreated = await _orderRepository.CreateAsync(order);

                //after order creation clear that particular cart.
                foreach (var cartItem in cartItems)
                {
                    await _cartRepository.DeleteAsync(cartItem.CartItemId);
                }

                return MapToDto(orderCreated);
            });
        }

        public async Task<List<OrderDto>> GetMyOrdersAsync(int userId)
        {
            var order = await _orderRepository.GetByUserIdAsync(userId);

            return order.Select(MapToDto).ToList();
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
                return null;

            if (order.UserId != userId) // checking whether the userId is the as the requested one
                return null;

            return MapToDto(order);
        }

        public async Task<List<OrderDto>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllAsync();

            return orders.Select(MapToDto).ToList();
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            return await _orderRepository.UpdateStatusAsync(orderId, status);
        }

        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,

                OrderItems = order.OrderItems.Select(item => new OrderItemDto
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Total = item.Price * item.Quantity
                }).ToList()
            };
        }
    }
}
