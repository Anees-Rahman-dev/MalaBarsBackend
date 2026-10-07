using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        //Dashboard

        public async Task<int> GetTotalUsersAsync()
        {
            return await _context.Users.CountAsync();
        }

        public async Task<int> GetTotalProductsAsync()
        {
            return await _context.Products.CountAsync();
        }

        public async Task<int> GetTotalOrdersAsync()
        {
            return await _context.orders.CountAsync();
        }

        public async Task<Decimal> GetTotalRevenueAsync()
        {
            return await _context.orders
                .Where(O => O.Status != "Cancelled")
                .SumAsync(O => O.TotalAmount);
        } 

        public async Task<int> GetLowStockCountAsync(int threshold = 5)
        {
            return await _context.Products
                .CountAsync(P => P.Stock <= threshold);
        }

        public async Task<List<Order>> GetRecentOrdersAsync(int count = 5)
        {
            return await _context.orders
                .Include(O => O.User)
                .OrderByDescending(O => O.OrderDate)
                .Take(count)
                .ToListAsync();
        }

        //User management

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<bool> ToggleBlockUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            user.IsBlocked = !user.IsBlocked; // Toggle the block status true and false;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> ChangeUserRoleAsync(int id, string role)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            user.Role = role;
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
                return false;

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<List<Order>> GetOrdersByUserIdAsync(int userId)
        {
            return await _context.orders
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product)
                .Include(O => O.User)
                .Where(O => O.UserId == userId)
                .OrderByDescending(O => O.OrderDate)
                .ToListAsync();
        }

        //Order management

        public async Task<List<Order>> GetAllOrdersAsync()
        {
            return await _context.orders
                .Include(O => O.User)
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product)
                .OrderByDescending(O => O.OrderDate)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderByIdAsync(int id)
        {
            return await _context.orders
                .Include(O => O.User)
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product)
                .FirstOrDefaultAsync(O => O.OrderId == id);
        }

        public async Task<int> GetPendingOrdersCountAsync()
        {
            return await _context.orders.CountAsync(O => O.Status == "Pending");
        }
        public async Task<bool> UpdateOrderStatusAsync(int id, string status)
        {
            var order = await _context.orders.FindAsync(id);

            if (order == null)
                return false;

            order.Status = status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
