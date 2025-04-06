using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface IProductSvc
    {
        Task<List<Product>> GetAllProductsAsync();
        Task<Product?> GetProductAsync(int id);
        Task<bool> AddProductAsync(Product product);
        Task<bool> EditProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
    }

    public class ProductSvc : IProductSvc
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductSvc> _logger;

        public ProductSvc(ApplicationDbContext context, ILogger<ProductSvc> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetProductAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<bool> AddProductAsync(Product product)
        {
            if (product == null) return false;

            try
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding product: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EditProductAsync(int id, Product product)
        {
            try
            {
                var existingProduct = await _context.Products.FindAsync(id);
                if (existingProduct == null) return false;

                existingProduct.ProductName = product.ProductName;
                existingProduct.UnitPrice = product.UnitPrice;
                existingProduct.CategoryId = product.CategoryId;
                existingProduct.Image = product.Image;
                existingProduct.Quantity = product.Quantity;

                await _context.SaveChangesAsync(); // No need to call Update()
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            try
            {
                var product = await _context.Products.FindAsync(id);
                if (product == null) return false;

                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {id}: {ex.Message}");
                return false;
            }
        }
    }
}
