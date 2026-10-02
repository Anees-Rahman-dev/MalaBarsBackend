using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class CreateOrderDto
    {
        public int AddressId { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
    }
}
