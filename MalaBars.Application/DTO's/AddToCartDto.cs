using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.DTO_s
{
    public class AddToCartDto //AddToCartDto is what the frontend sends:ProductId = 5 ,Quantity = 2
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
