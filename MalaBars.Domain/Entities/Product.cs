using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Domain.Entities
{
    public class Product
    {

        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public int Stock { get; set; }

        public decimal Rating { get; set; }
    }
}
