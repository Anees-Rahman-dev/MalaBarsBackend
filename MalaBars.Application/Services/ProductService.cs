using MalaBars.Application.Interfaces;
using MalaBars.Application.DTO_s;
using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;

namespace MalaBars.Application.Services;

public class ProductService : IProductService
{
private readonly IProductRepository _productRepository;

public ProductService(IProductRepository productRepository)
{
    _productRepository = productRepository;
}

    public async Task<List<ProductDto>> GetAllAsync()
    {
        var products = await _productRepository.GetAllAsync();

        return products.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            Name = p.Name,
            Category = p.Category,
            Price = p.Price,
            Description = p.Description,
            Image = p.Image,
            Stock = p.Stock,
            Rating = p.Rating
        }).ToList();
    }

    public async Task<ProductDto> GetByIdAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);

        if (product == null)
        {
            return null;
        }else
        {
            return new ProductDto
            {

                ProductId = product.ProductId,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category,
                Description = product.Description,
                Image = product.Image,
                Stock = product.Stock,
                Rating = product.Rating

            };
        }
    }

    public async Task<ProductDto> CreateAsync(ProductDto product)
    {
        var newProduct = new Product
        {

            Name = product.Name,
            Category = product.Category,
            Price = product.Price,
            Description = product.Description,
            Image = product.Image,
            Stock = product.Stock,
            Rating = product.Rating

        };

        var createdProduct = await _productRepository.AddAsync(newProduct);

        return new ProductDto
        {

            ProductId = createdProduct.ProductId,
            Name = createdProduct.Name,
            Category = createdProduct.Category,
            Price = createdProduct.Price,
            Description = createdProduct.Description,
            Image = createdProduct.Image,
            Stock = createdProduct.Stock,
            Rating = createdProduct.Rating
        };
    }

    public async Task<bool> UpdateAsync(int id, ProductDto product)
    {
        var existingProduct = await _productRepository.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return false;
        }
        else
        {
            existingProduct.Name = product.Name;
            existingProduct.ProductId = product.ProductId;
            existingProduct.Category = product.Category;
            existingProduct.Price = product.Price;
            existingProduct.Description = product.Description;
            existingProduct.Rating = product.Rating;
            existingProduct.Stock = product.Stock;
            existingProduct.Image = product.Image;
        }
        return await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}
