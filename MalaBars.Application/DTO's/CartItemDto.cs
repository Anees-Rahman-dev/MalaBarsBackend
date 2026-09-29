using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class CartItemDto //CartItemDto is what the backend returns: ProductId = 5, ProductName = Dark Chocolate, Price = 250, Quantity = 2 ,Total = 500
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Total { get; set; }
    }
}
