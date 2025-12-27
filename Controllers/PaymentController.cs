// PaymentController.cs
using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.DTOs;
using car_marketplace_api_3tier.Repositories;
using car_marketplace_api_3tier.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace car_marketplace_api_3tier.Controllers
{
    [ApiController]
    [Route("[controller]")] // المسار هو /Payment
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly ICartRepository _cartRepository;
        private readonly IPaymentService _paymentService;

        public PaymentController(ICartRepository cartRepository, IPaymentService paymentService)
        {
            _cartRepository = cartRepository;
            _paymentService = paymentService;
        }

        private string GetUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        // POST /Payment/checkout
        // يستخدم لتحويل السلة إلى طلب وإعادة توجيه المستخدم لصفحة دفع Stripe
        [HttpPost("checkout")]
        public async Task<ActionResult<string>> Checkout([FromBody] CheckoutRequestDto request)
        {
            var userId = GetUserId();

            // 1. تحويل سلة المشتريات إلى Order (حالة Pending)
            var order = await _cartRepository.CreateOrderFromCartAsync(userId, request.ShippingAddress);

            if (order == null)
            {
                return BadRequest("Your cart is empty or the order could not be created.");
            }

            // 2. إنشاء جلسة دفع Stripe
            // يجب تحديد نطاق (Domain) التطبيق الأمامي (Frontend)
            var domain = "http://localhost:5173";
            var session = await _paymentService.CreateCheckoutSessionAsync(order, domain);

            // 3. إعادة رابط URL الخاص بجلسة Stripe إلى الفرونت-اند
            return Ok(new { url = session.Url });
        }
    }
}