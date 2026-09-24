using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Domain.Entities
{
    public class WishlistItem
    {
        public int WishlistItemId { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public User User { get; set; } = null!;

        public Product Product { get; set; } = null!;
    }
}
