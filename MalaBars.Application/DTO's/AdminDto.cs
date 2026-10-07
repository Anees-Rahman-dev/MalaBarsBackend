using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    // ── Dashboard ────────────────────────────────────────────────
    public class DashboardStatsDto
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int PendingOrders { get; set; }
        public int LowStockProducts { get; set; }
        public List<RecentOrderDto> RecentOrders { get; set; } = new();
    }

    public class RecentOrderDto
    {
        public int OrderId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
    }

    // ── User management ──────────────────────────────────────────
    public class AdminUserDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public bool IsBlocked { get; set; }
        public int TotalOrders { get; set; }
    }

    public class ChangeRoleDto
    {
        public string Role { get; set; } = string.Empty;
    }

    // ── Order management ─────────────────────────────────────────
    public class AdminOrderDto
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }

    public class UpdateOrderStatusDto
    {
        public string Status { get; set; } = string.Empty;
    }
}