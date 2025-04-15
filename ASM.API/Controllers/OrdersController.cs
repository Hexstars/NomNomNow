using ASM.Share.Models.Responses;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASM.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderSvc _orderSvc;
        private readonly ICartSvc _cartSvc;
        private readonly IAccountSvc _accountSvc;
        public OrdersController(IOrderSvc orderSvc, ICartSvc cartSvc, IAccountSvc accountSvc)
        {
            _orderSvc = orderSvc;
            _cartSvc = cartSvc;
            _accountSvc = accountSvc;
        }
        [HttpGet("GetDetails")]
        public async Task<IActionResult> GetDetails()
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            var account = await _accountSvc.GetAccount(userId);
            var cartDetails = await _cartSvc.GetCartItemsAsync(userId); // or by CartId
            return Ok(new ConfirmOrder
            {
                Account = account,
                CartProducts = cartDetails
            });
        }
    }
}
