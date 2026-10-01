using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository 
    {
        private readonly ApplicationDbContext _context;

        public AddressRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<Address>> GetByUserIdAsync(int userId)
        {
            return await _context.Addresses
                .Where(A => A.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetByIdAsync(int addressId)
        {
            return await _context.Addresses.FirstOrDefaultAsync(A => A.AddressId == addressId);
        }

        public async Task<Address> AddAsync(Address address)
        {
            _context.Addresses.Add(address);

            await _context.SaveChangesAsync();

            return address;
        }

        public async Task<bool> UpdateAsync(Address address)
        {
            var existingAddress = await _context.Addresses.FindAsync(address.AddressId);

            if (existingAddress == null)
                return false;

            existingAddress.FullName = address.FullName;
            existingAddress.Phone = address.Phone;
            existingAddress.AddressLine = address.AddressLine;
            existingAddress.City = address.City;
            existingAddress.State = address.State;
            existingAddress.Pincode = address.Pincode;

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int addressId)
        {
            var address = await _context.Addresses.FindAsync(addressId);

            if(address == null)
            {
                return false;
            }
            else
            {
                _context.Addresses.Remove(address);
                await _context.SaveChangesAsync();
            }
            return true;
        }

    }
}
