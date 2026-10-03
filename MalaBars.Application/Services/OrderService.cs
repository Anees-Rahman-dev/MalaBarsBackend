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
        private readonly IAddressRepository _addressRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IOrderRepository orderRepository,
        ICartRepository cartRepository,
        IProductRepository productRepository,
        IAddressRepository addressRepository,
        IPaymentRepository paymentRepository,
        IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _addressRepository = addressRepository;
            _paymentRepository = paymentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<OrderDto?> CreateOrderAsync(int userId,CreateOrderDto request)
        {
            return await _unitOfWork.ExecuteInTransactionAsync(async () =>
            {

                //1- Validate address
                var address = await _addressRepository.GetByIdAsync(request.AddressId);

                if (address == null || address.UserId != userId)
                    throw new Exception("Invalid Address");

                //2- Get cart items for the user and check if there are any items in the cart
                    
                var cartItems = await _cartRepository.GetByUserIdAsync(userId);

                if (!cartItems.Any())

                    return null;

                //3- validate payment method

                var allowedPaymentMethods = new[]
                {
                    "COD",
                    "UPI",
                    "Card"
                };

                if(!allowedPaymentMethods.Contains(
                    request.PaymentMethod, StringComparer.OrdinalIgnoreCase))
                {
                    throw new Exception("Invalid payment method.");
                }

                decimal totalAmount = 0;

                var order = new Order
                {
                    UserId = userId,
                    AddressId = request.AddressId,
                    OrderDate = DateTime.UtcNow,
                    Status = "Pending"
                };

                //4 - Create Order Items + reduce Stock


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

                //5- Set total
                order.TotalAmount = totalAmount;

                //6- Create order
                var createdOrder = await _orderRepository.CreateAsync(order);

                createdOrder.Address = address ; // setting the address to the order so that it can be returned in the response.

                //7- Create payment
                var payment = new Payment
                {
                    OrderId = createdOrder.OrderId,
                    Amount = totalAmount,
                    PaymentMethod = request.PaymentMethod,
                    PaymentStatus = request.PaymentMethod.Equals
                    ("COD", StringComparison.OrdinalIgnoreCase)
                    ? "Pending"
                    : "Paid",
                    PaymentDate = DateTime.UtcNow
                };
                await _paymentRepository.AddAsync(payment);


                //8- Clear cart
                //after order creation clear that particular cart.
                foreach (var cartItem in cartItems)
                {
                    await _cartRepository.DeleteAsync(cartItem.CartItemId);
                }

                return MapToDto(createdOrder);
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

            var allowedStatuses = new[]
            {
                "Pending",
                "Confirmed",
                "Shipped",
                "Delivered",
                "Cancelled"
            };

            if (!allowedStatuses.Contains(status,StringComparer.OrdinalIgnoreCase))
            {
                return false;
            }

            return await _orderRepository.UpdateStatusAsync(orderId, status);
        }


        public async Task<OrderDto?> GetAdminOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);

            if (order == null)
            {
                return null;
            }
            else
            {
                return MapToDto(order);
            }
        }
        private static OrderDto MapToDto(Order order)   
        {
            return new OrderDto
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status,

                Address = order.Address == null ? null : new OrderAddressDto
                {
                    FullName = order.Address.FullName,
                    Phone = order.Address.Phone,
                    AddressLine = order.Address.AddressLine,
                    City = order.Address.City,
                    State = order.Address.State,
                    Pincode = order.Address.Pincode
                },

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
