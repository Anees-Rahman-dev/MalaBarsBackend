using MalaBars.Application.Interfaces;
using MalaBars.Domain.Entities;
using MalaBars.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MalaBars.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<Product> AddAsync(Product product)
        {
            _context.Products.Add(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateAsync( Product product)
        {
            var ExistingProduct = await _context.Products.FindAsync(product.ProductId);

            if (ExistingProduct == null)
            {
                return false;
            }
            else
            {
                ExistingProduct.Name = product.Name;
                ExistingProduct.Price = product.Price;
                ExistingProduct.Category = product.Category;
                ExistingProduct.Rating = product.Rating;
                ExistingProduct.Description = product.Description;
                ExistingProduct.Stock = product.Stock;
                ExistingProduct.Image = product.Image;

                await _context.SaveChangesAsync();
            }
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var FindProduct = await _context.Products.FindAsync(id);

            if (FindProduct == null)
            {
                return false;
            }
            else
            {

                _context.Remove(FindProduct);

                await _context.SaveChangesAsync();
            }
            return true;
        }
    }
}
