using ASM.Share.Models;
using ASM.Share.Models.Responses;
using ASM.Share.Models.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http.HttpResults;
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
        [HttpGet("GetOrders")]
        public async Task<ActionResult<List<OrderView>>> GetOrders()
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            var orders = await _orderSvc.GetAllOrders(userId);
            return Ok(orders);
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder()
        {
            // Check if the user is authenticated
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized("Invalid user ID in token");
            }
            else
            {
                var user = await _accountSvc.GetAccount(userId);
                Order order = new Order
                {
                    AccountId = user.AccountId,
                    OrderDate = DateTime.Now,
                    Phone = user.Phone,
                    Address = user.Address,
                    Status = 0,
                };

                //add order
                bool result = await _orderSvc.CreateOrder(order);
                if (result)
                {
                    Cart cart = await _cartSvc.GetUserCart(userId);

                    var products = await _cartSvc.GetCartItemsAsync(cart.CartId);

                    if (products.Any())
                    {
                        // Add products to the order details
                        await _orderSvc.AddIntoDetail(products, order.OrderId);

                        // Clear the cart after adding items to the order
                        await _cartSvc.ClearCartAsync(cart.CartId);
                    }
                }
                else
                {
                    return BadRequest("Failed to create order.");
                }
            }
            return Ok("Order created successfully.");
        }

        [HttpGet("GetDetails")]
        public async Task<IActionResult> GetDetails() //xem ở /confirm-order
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
