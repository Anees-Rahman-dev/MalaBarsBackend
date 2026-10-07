using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IAdminRepository
    {
         Task<int> GetTotalUsersAsync();
        Task<int> GetTotalProductsAsync();
        Task<int> GetTotalOrdersAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetPendingOrdersCountAsync();
        Task<int> GetLowStockCountAsync(int threshold = 5);
        Task<List<Order>> GetRecentOrdersAsync(int count = 5);

        // User management
        Task<List<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> ToggleBlockUserAsync(int id);
        Task<bool> ChangeUserRoleAsync(int id, string role);
        Task<bool> DeleteUserAsync(int id);
        Task<List<Order>> GetOrdersByUserIdAsync(int userId);

        // Order management
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, string status);
    }
}
