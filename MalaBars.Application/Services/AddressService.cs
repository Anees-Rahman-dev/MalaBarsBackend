using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace MalaBars.Application.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService (IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<List<AddressDto>> GetMyAddressesAsync(int userId)
        {
            var addresses = await _addressRepository.GetByUserIdAsync(userId);

            return addresses.Select(MapToDto).ToList();
        }

        public async Task<AddressDto?> GetByIdAsync(int userId, int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);

            if (address == null || address.UserId != userId)
                return null;

            return MapToDto(address);
        }


        public async Task<AddressDto> AddAsync(int userId, AddressDto request)
        {
            var address = new Address
            {
                UserId = userId,
                FullName = request.FullName,
                Phone = request.Phone,
                AddressLine = request.AddressLine,
                City = request.City,
                State = request.State,
                Pincode = request.Pincode
            };

            var createdAddress = await _addressRepository.AddAsync(address);

            return MapToDto(createdAddress);
        }

        public async Task<bool> UpdateAsync(int userId, int addressId, AddressDto request)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);

            if(address == null || address.UserId != userId)
            {
                return false;
            }
            else
            {
                address.FullName = request.FullName; ////upadting already existing adress
                address.Phone = request.Phone;
                address.Pincode = request.Pincode;
                address.State = request.State;
                address.AddressLine = request.AddressLine;
                address.City = request.City;

                return await _addressRepository.UpdateAsync(address); //upadted the Db, which is already existing adress
            }
        }


        public async Task<bool> DeleteAsync(int userId, int addressId)
        {
            var address = await _addressRepository.GetByIdAsync(addressId);

            if(address == null || address.UserId != userId)
            {
                return false;
            }
            else
            {
                return await _addressRepository.DeleteAsync(addressId);
            }
        }

        private static AddressDto MapToDto(Address address)
        {
            return new AddressDto
            {
                AddressId = address.AddressId,
                FullName = address.FullName,
                Phone = address.Phone,
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State,
                Pincode = address.Pincode
            };
        }
    }
}
