using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepo;

        public AdminService(IAdminRepository adminRepo)
        {
            _adminRepo = adminRepo;
        }

        //Dashboard
        public async Task<DashboardStatsDto> GetDashboardStatsAsync()
        {
            var recentOrders = await _adminRepo.GetRecentOrdersAsync(5);
            return new DashboardStatsDto
            {
                TotalUsers = await _adminRepo.GetTotalUsersAsync(),
                TotalProducts = await _adminRepo.GetTotalProductsAsync(),
                TotalOrders = await _adminRepo.GetTotalOrdersAsync(),
                TotalRevenue = await _adminRepo.GetTotalRevenueAsync(),
                PendingOrders = await _adminRepo.GetPendingOrdersCountAsync(),
                LowStockProducts = await _adminRepo.GetLowStockCountAsync(),
                RecentOrders = recentOrders.Select(O => new RecentOrderDto
                {
                    OrderId = O.OrderId,
                    UserName = O.User.Name,
                    TotalAmount = O.TotalAmount,
                    Status = O.Status,
                    OrderDate = O.OrderDate
                }).ToList()
            };
        }

        //User Management
        public async Task<List<AdminUserDto>> GetAllUsersAsync()
        {
            var users = await _adminRepo.GetAllUsersAsync();
            var result = new List<AdminUserDto>();

            foreach(var user in users)
            {
                var orders = await _adminRepo.GetOrdersByUserIdAsync(user.UserId);
                result.Add(MapUserToDto(user, orders.Count));
            }
            return result;
        }

        public async Task<List<AdminOrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _adminRepo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapOrderDto).ToList();
        }

        public async Task<AdminUserDto?> GetUserByIdAsync(int id)
        {
            var user = await _adminRepo.GetUserByIdAsync(id);
            if (user == null)
                return null;

            var orders = await _adminRepo.GetOrdersByUserIdAsync(id);
            return MapUserToDto(user, orders.Count);
        }

        public async Task<bool> ToggleBlockUserAsync(int id)
        {
            return await _adminRepo.ToggleBlockUserAsync(id);
        }

        public async Task<bool> ChangeUserRoleAsync(int id, ChangeRoleDto dto)
        {
            var validRoles = new[] { "User", "Admin"};

            if (!validRoles.Contains(dto.Role))
                throw new Exception("Invalid role. Must be 'User' or 'Admin'");

            return await _adminRepo.ChangeUserRoleAsync(id, dto.Role);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _adminRepo.DeleteUserAsync(id);
        }

        public async Task<List<AdminOrderDto>> GetOrdersByUserIdAsync(int userId)
        {
            var orders = await _adminRepo.GetOrdersByUserIdAsync(userId);
            return orders.Select(MapOrderDto).ToList();
        }

        //Order management

        public async Task<List<AdminOrderDto>> GetAllOrdersAsync()
        {
            var orders = await _adminRepo.GetAllOrdersAsync();
            return orders.Select(MapOrderDto).ToList();
        }

        public async Task<AdminOrderDto?> GetOrderByIdAsync(int id)
        {
            var order = await _adminRepo.GetOrderByIdAsync(id);
            return order == null ? null : MapOrderDto(order);
        }

        public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto dto)
        {
            var validStatuses = new[] {"Pending", "Shipped", "Delivered", "Cancelled"};
            if (!validStatuses.Contains(dto.Status))
                throw new Exception("Invalied Status");

            return await _adminRepo.UpdateOrderStatusAsync(id,dto.Status);
        }
        //Mappers
        private static AdminUserDto MapUserToDto(User u, int orderCount) => new()
        { 
            // Mapper is f
            UserId = u.UserId,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role,
            IsBlocked = u.IsBlocked,
            TotalOrders = orderCount
        };
        public static AdminOrderDto MapOrderDto(Order o)
        {
            return new()
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                UserName = o.User.Name,
                UserEmail = o.User.Email,
                OrderDate = o.OrderDate,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Items = o.OrderItems.Select(oi => new OrderItemResponseDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Subtotal = oi.Price * oi.Quantity
                }).ToList()
            };
        }
    }
}
