using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;  // Added for logging

namespace ASM.Share.Models.Services
{
    public interface ICategorySvc
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<Category?> GetCategoryAsync(int id);
        Task<bool> AddCategoryAsync(Category category);
        Task<bool> EditCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }

    public class CategorySvc : ICategorySvc
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategorySvc> _logger;  // Added logging

        public CategorySvc(ApplicationDbContext context, ILogger<CategorySvc> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> AddCategoryAsync(Category category)
        {
            if (category == null) return false; // Prevent adding null category

            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding category: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> EditCategoryAsync(int id, Category category)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null) return false;

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Image = category.Image;

                await _context.SaveChangesAsync(); // No need to call Update()
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating category {id}: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            try
            {
                var category = await _context.Categories.FindAsync(id);
                if (category == null) return false;

                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting category {id}: {ex.Message}");
                return false;
            }
        }
    }
}
