using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Share.Models;
using ASM.Share.Models.Services;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategorySvc _categorySvc;

        public CategoriesController(ICategorySvc categorySvc)
        {
            _categorySvc = categorySvc;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return Ok(await _categorySvc.GetAllCategoryAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categorySvc.GetCategoryAsync(id);
            if (category == null) return NotFound();
            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            var id = await _categorySvc.AddCategoryAsync(category);
            if (id == 0) return BadRequest();
            return CreatedAtAction(nameof(GetCategory), new { id }, category);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, Category category)
        {
            if (id != category.CategoryId) return BadRequest();
            var result = await _categorySvc.EditCategoryAsync(id, category);
            if (result == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var deleted = await _categorySvc.DeleteCategoryAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
}
