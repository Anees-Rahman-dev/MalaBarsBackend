using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);

        Task<User?> GetByIdAsync(int id);

        Task<List<User>> GetAllAsync();
        Task<User> AddAsync(User user);

        Task<bool> BlockAsync(int id);

        Task<bool> UnblockAsync(int id);
    }
}
