using ASM.Share.Models;
using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace ASM.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartSvc _cartSvc;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartController(ICartSvc cartSvc, IHttpContextAccessor httpContextAccessor)
        {
            _cartSvc = cartSvc;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ActionResult<List<CartProduct>>> Index()
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value);
            if (userId == null)
            {
                return Unauthorized();
            }
            return await _cartSvc.GetCartItemsAsync(userId);
        }
    }
}
