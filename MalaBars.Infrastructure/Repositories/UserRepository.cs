using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(P => P.Email == email);
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<List<User>> GetAllAsync()
        {
          return await _context.Users.ToListAsync();
        } 

        public async Task<bool> BlockAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if(user == null)
            {
                return false;
            }
            else
            {
                user.IsBlocked = true;
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UnblockAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return false;
            }
            else
            {
            user.IsBlocked = false;
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}
