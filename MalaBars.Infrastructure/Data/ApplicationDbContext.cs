using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using MalaBars.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MalaBars.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }

        public DbSet<CartItem> cartItems { get; set; }

        public DbSet<WishlistItem> wishlistItems { get; set; }

        public DbSet<Order> orders { get; set; }

        public DbSet<OrderItem> orderItems { get; set; }

    }
}
