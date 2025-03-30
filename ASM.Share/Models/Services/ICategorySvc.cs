using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASM.Share.Models.Services
{
    public interface ICategorySvc
    {
        Task<List<Category>> GetAllCategoryAsync();
        Task<Category> GetCategoryAsync(int id);
        Task<int> AddCategoryAsync(Category category);
        Task<int> EditCategoryAsync(int id, Category category);
        Task<bool> DeleteCategoryAsync(int id);
    }

    public class CategorySvc : ICategorySvc
    {
        private readonly ApplicationDbContext _context;

        public CategorySvc(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetAllCategoryAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategoryAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<int> AddCategoryAsync(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                await _context.SaveChangesAsync();
                return category.CategoryId;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<int> EditCategoryAsync(int id, Category category)
        {
            try
            {
                var existingCategory = await _context.Categories.FindAsync(id);
                if (existingCategory == null) return 0;

                existingCategory.CategoryName = category.CategoryName;
                existingCategory.Image = category.Image;

                _context.Categories.Update(existingCategory);
                await _context.SaveChangesAsync();
                return existingCategory.CategoryId;
            }
            catch
            {
                return 0;
            }
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
