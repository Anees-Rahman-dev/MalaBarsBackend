using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.orders.Add(order);

            await _context.SaveChangesAsync();

            return order;
        }

        public async Task<List<Order>> GetByUserIdAsync(int userId)
        {
            return await _context.orders
                .Include(O => O.Address)
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product)  //Order > OrderItems > Product
                .Where(O => O.UserId == userId)
                .ToListAsync();
        }

        // we use then  So when a user views an order, we can return something like:

 //     Order #1
 //     ├── Dark Chocolate × 2
 //     ├── Nut Fusion × 1
 //     └── Total: ₹750

        public async Task<Order?> GetByIdAsync(int orderId)
        {
            return await _context.orders
                .Include(o => o.Address)
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product) ////Order > OrderItems > Product
                .FirstOrDefaultAsync(O => O.OrderId == orderId);
        }

        public async Task<List<Order>> GetAllAsync()
        {
            return await _context.orders
                .Include(o => o.Address)
                .Include(O => O.OrderItems)
                .ThenInclude(O => O.Product)// //Order > OrderItems > Product
                .ToListAsync();
        }

        public async Task<bool> UpdateStatusAsync(int orderId, string status)
        {
            var order = await _context.orders.FindAsync(orderId);

            if (order == null)
                return false;

            order.Status = status;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
