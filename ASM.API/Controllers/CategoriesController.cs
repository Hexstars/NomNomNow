using ASM.Share.Models;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ICategorySvc _categorySvc;
        private readonly IUploadHelper _uploadHelper;

        public CategoriesController(ICategorySvc categorySvc, IUploadHelper uploadHelper, IWebHostEnvironment webHostEnvironment)
        {
            _categorySvc = categorySvc;
            _uploadHelper = uploadHelper;
            _webHostEnvironment = webHostEnvironment;
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
        public async Task<ActionResult<Category>> PostCategory([FromForm] Category category)
        {
            if (category.ImageFile != null)
            {
                if (category.ImageFile.Length > 0)
                {
                    // Kiểm tra WebRootPath
                    //if (string.IsNullOrEmpty(_webHostEnvironment.WebRootPath))
                    //{
                    //    return BadRequest("WebRootPath is not configured correctly.");
                    //}

                    // Đường dẫn lưu trữ file
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Kiểm tra và tải ảnh lên
                    await _uploadHelper.UploadImage(category.ImageFile, rootPath, "Categories");

                    // Lưu tên file vào Image (tên file là ImageFile.Name)
                    category.Image = category.ImageFile.FileName;

                    // Thêm danh mục vào cơ sở dữ liệu
                    var result = await _categorySvc.AddCategoryAsync(category);
                    if (!result)
                    {
                        return BadRequest("Không thể thêm danh mục.");
                    }
                    return Ok();
                }
                else
                {
                    return BadRequest("File không hợp lệ.");
                }
            }
            else
            {
                return BadRequest("Vui lòng chọn hình ảnh.");
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromForm] Category category)
        {
            if (id != category.CategoryId) return BadRequest();

            // Check if the image file exists and is valid
            if (category.ImageFile != null)
            {
                if (category.ImageFile.Length > 0)
                {
                    // Path to save the image
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Upload the image (using your helper or custom method)
                    await _uploadHelper.UploadImage(category.ImageFile, rootPath, "Categories");

                    // Update the category image filename in the database
                    category.Image = category.ImageFile.FileName;
                }
                else
                {
                    return BadRequest("File không hợp lệ.");
                }
            }

            // Call the service to update the category in the database
            var result = await _categorySvc.EditCategoryAsync(id, category);
            if (!result) return NotFound();

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
