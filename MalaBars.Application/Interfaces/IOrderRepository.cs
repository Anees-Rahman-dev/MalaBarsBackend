using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> CreateAsync(Order order);

        Task<List<Order>> GetByUserIdAsync(int userId); //User sees their own orders

        Task<Order?> GetByIdAsync(int orderId); //View one order

        Task<List<Order>> GetAllAsync(); //Admin sees all orders

        Task<bool> UpdateStatusAsync(int orderId, string status); //Admin changes order status
    }
}
