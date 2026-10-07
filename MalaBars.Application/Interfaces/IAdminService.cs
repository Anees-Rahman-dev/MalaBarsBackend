using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IAdminService
    {
        // Dashboard
        Task<DashboardStatsDto> GetDashboardStatsAsync();

        // User management
        Task<List<AdminUserDto>> GetAllUsersAsync();
        Task<AdminUserDto?> GetUserByIdAsync(int id);
        Task<bool> ToggleBlockUserAsync(int id);
        Task<bool> ChangeUserRoleAsync(int id, ChangeRoleDto dto);
        Task<bool> DeleteUserAsync(int id);
        Task<List<AdminOrderDto>> GetUserOrdersAsync(int userId);

        // Order management
        Task<List<AdminOrderDto>> GetAllOrdersAsync();
        Task<AdminOrderDto?> GetOrderByIdAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto);

    }
}
