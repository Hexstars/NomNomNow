using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASM.Share.Models;
using Microsoft.AspNetCore.Hosting;
using ASM.Share.Models.Services;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IProductSvc _productSvc;
        private readonly IUploadHelper _uploadHelper;

        public ProductsController(ApplicationDbContext context, IUploadHelper uploadHelper, IWebHostEnvironment webHostEnvironment, IProductSvc productSvc)
        {
            _context = context;
            _uploadHelper = uploadHelper;
            _webHostEnvironment = webHostEnvironment;
            _productSvc = productSvc;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productSvc.GetAllProductsAsync());
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return BadRequest();
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct(Product product)
        {
            if (product.ImageFile != null)
            {
                if (product.ImageFile.Length > 0)
                {
                    // Đường dẫn lưu trữ file
                    string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "images");

                    // Kiểm tra và tải ảnh lên
                    await _uploadHelper.UploadImage(product.ImageFile, rootPath, "Products");

                    // Lưu tên file vào Image (tên file là ImageFile.Name)
                    product.Image = product.ImageFile.FileName;

                    // Thêm danh mục vào cơ sở dữ liệu
                    var result = await _productSvc.AddProductAsync(product);
                    if (!result)
                    {
                        return BadRequest("Không thể thêm sản phẩm.");
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

        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
