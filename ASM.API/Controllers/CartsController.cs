using ASM.Share.Models;
using ASM.Share.Models.Requests;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ASM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly ICartSvc _cartSvc;
        public CartsController(ICartSvc cartSvc)
        {
            _cartSvc = cartSvc;
        }
        [AllowAnonymous]
        [HttpGet("validate-token")]
        public IActionResult ValidateToken([FromHeader(Name = "Authorization")] string authHeader)
        {
            var token = authHeader?.Replace("Bearer ", "");
            var handler = new JwtSecurityTokenHandler();

            try
            {
                var principal = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidIssuer = "SecureServerAPI",
                    ValidAudience = "SecureClientAPI",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("7c37456658159943f5c8fbbba79178eebef6e87cb1c0aca5be828e95b8ea3fbb")),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // Disable tolerance temporarily
                }, out _);

                return Ok(new
                {
                    Success = true,
                    UserId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                    Claims = principal.Claims.Select(c => new { c.Type, c.Value })
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Error = ex.Message,
                    Token = token,
                    CurrentTime = DateTime.UtcNow
                });
            }
        }
        [HttpGet]
        public async Task<ActionResult<List<CartProduct>>> Index()
        {
            // Use ClaimTypes.NameIdentifier to match your token creation
            //Console.WriteLine($"All claims: {JsonSerializer.Serialize(User.Claims)}"); // Debug
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            return await _cartSvc.GetCartItemsAsync(userId);
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartRequest request)
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            Cart cart = await _cartSvc.GetUserCart(int.Parse(userIdClaim));
            bool added = await _cartSvc.AddToCart(cart.AccountId, request.ProductId, request.Quantity);

            if (added)
            {
                return Ok(new { Message = "ĐÃ THÊM SẢN PHẨM VÀO GIỎ HÀNG" });
            }
            else
            {
                return BadRequest(new { Message = "THÊM VÀO GIỎ HÀNG THẤT BẠI" });
            }
            
        }
        [HttpPut("update-quantity")]
        public async Task<IActionResult> UpdateQuantity([FromBody] AddToCartRequest request)
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            bool updated = await _cartSvc.UpdateQuantity(userId, request.ProductId, request.Quantity);
            if (updated)
            {
                return Ok(new { Message = "CẬP NHẬT SỐ LƯỢNG THÀNH CÔNG" });
            }
            else
            {
                return BadRequest(new { Message = "CẬP NHẬT SỐ LƯỢNG THẤT BẠI" });
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            bool deleted = await _cartSvc.DeleteFromCart(userId, productId);
            if (deleted)
            {
                return Ok(new { Message = "XÓA SẢN PHẨM THÀNH CÔNG" });
            }
            else
            {
                return BadRequest(new { Message = "XÓA SẢN PHẨM THẤT BẠI" });
            }
        }
    }
}
