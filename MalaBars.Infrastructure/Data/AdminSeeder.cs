using MalaBars.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Data
{
    public class AdminSeeder
    {
        public static async Task SeedAdminAsync(ApplicationDbContext context)
        {
            var adminExists = await context.Users
                .AnyAsync(U => U.Role == "Admin"); // Check if an admin user already exists

            if (adminExists) // If an admin user already exists, we don't need to seed another one
                return;

            var passwrodHasher = new PasswordHasher<User>();

            var admin = new User
            {
                Name = "Admin",
                Email = "admin@malabars.com",
                Role = "Admin",
                IsBlocked = false
            };

            admin.PasswordHash = passwrodHasher.HashPassword(
                admin,
                "Admin@123");

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }
}
