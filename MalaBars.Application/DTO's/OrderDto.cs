using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = string.Empty;

        public OrderAddressDto? Address { get; set; }

        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
