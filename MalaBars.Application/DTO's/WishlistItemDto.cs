using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class WishlistItemDto
    {
        public int WishlistItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string Image { get; set; } = string.Empty;
    }
}
