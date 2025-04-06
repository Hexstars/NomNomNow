using ASM.Share.Models;
using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ICategorySvc _categorySvc;
        private readonly IProductSvc _productSvc;

        public HomeController(ICategorySvc categorySvc, IProductSvc productSvc)
        {
            _categorySvc = categorySvc;
            _productSvc = productSvc;
        }

        [HttpGet]
        public async Task<ActionResult<HomePage>> Index()
        {
            var products = await _productSvc.GetAllProductsAsync();
            var categories = await _categorySvc.GetAllCategoryAsync();

            var result = new HomePage
            {
                Products = products.ToList(),
                Categories = categories.ToList()
            };

            return Ok(result);
        }
    }
}
