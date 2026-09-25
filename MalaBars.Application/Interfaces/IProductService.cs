using MalaBars.Application.DTO_s;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Application.Interfaces
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetAllAsync();

        Task<ProductDto?> GetbyIdAsync(int id);

        Task<ProductDto> CreateAsync(ProductDto product);

        Task<bool> UpdateAsync(int id, ProductDto product);

        Task<bool> DeleteAsync(int id);

    }
}
