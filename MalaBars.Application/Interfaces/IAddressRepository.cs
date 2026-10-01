using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IAddressRepository
    {
        Task<List<Address>> GetByUserIdAsync(int userId);
        Task<Address?> GetByIdAsync(int addressId);
        Task<Address> AddAsync(Address address);
        Task<bool> UpdateAsync(Address address);
        Task<bool> DeleteAsync(int addressId);
    }
}
