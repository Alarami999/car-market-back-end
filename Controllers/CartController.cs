// CartController.cs
using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace car_marketplace_api_3tier.Controllers
{
    [ApiController]
    [Route("[controller]")] // المسار هو /Cart
    [Authorize] // يجب أن يكون المستخدم مسجلاً للدخول لاستخدام السلة
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId()
        {
            // استخراج الـ UserId من الـ JWT Token
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // GET /Cart
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartItem>>> GetCart()
        {
            var userId = GetUserId();
            var cartItems = await _cartService.GetCartAsync(userId);
            return Ok(cartItems);
        }

        // POST /Cart/add/{carId}
        // إضافة سيارة إلى السلة. يمكن أن تأتي الكمية اختيارياً من الـ Query
        [HttpPost("add/{carId}")]
        public async Task<ActionResult<CartItem>> AddToCart(int carId, [FromQuery] int quantity = 1)
        {
            var userId = GetUserId();
            var cartItem = await _cartService.AddCarToCartAsync(userId, carId, quantity);

            if (cartItem == null)
            {
                return NotFound("Car not found or quantity is invalid.");
            }

            return Ok(cartItem);
        }

        // DELETE /Cart/remove/{carId}
        [HttpDelete("remove/{carId}")]
        public async Task<ActionResult> RemoveFromCart(int carId)
        {
            var userId = GetUserId();
            var result = await _cartService.RemoveCarFromCartAsync(userId, carId);

            if (!result)
            {
                return NotFound("Item not found in cart.");
            }

            return NoContent();
        }

        // DELETE /Cart/clear
        [HttpDelete("clear")]
        public async Task<ActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartService.ClearUserCartAsync(userId);
            return NoContent();
        }
    }
}