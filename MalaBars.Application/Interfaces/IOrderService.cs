using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IOrderService
    {
        Task<OrderDto?> CreateOrderAsync(int userId);

        Task<List<OrderDto>> GetMyOrdersAsync(int userId);

        Task<OrderDto?> GetOrderByIdAsync(int userId, int orderId);

        Task<List<OrderDto>> GetAllOrdersAsync();

        Task<bool> UpdateStatusAsync(int orderId, string status);
    }
}
