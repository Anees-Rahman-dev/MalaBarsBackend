using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class BuyNowOrderDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int AddressId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
