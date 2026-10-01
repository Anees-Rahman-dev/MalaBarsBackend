using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IAddressService
    {
        Task<List<AddressDto>> GetMyAddressesAsync(int userId);
        Task<AddressDto?> GetByIdAsync(int userId, int addressId);
        Task<AddressDto> AddAsync(int userId, AddressDto request);
        Task<bool> UpdateAsync(int userId, int addressId, AddressDto request);
        Task<bool> DeleteAsync(int userId, int addressId);
    }
}
