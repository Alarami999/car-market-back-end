// StripeWebhookController.cs
using car_marketplace_api_3tier.Services;
using car_marketplace_api_3tier.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe;
using System.IO;

namespace car_marketplace_api_3tier.Controllers
{
    [Route("stripe-webhook")]
    [ApiController]
    public class StripeWebhookController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly string _webhookSecret;

        public StripeWebhookController(IPaymentService paymentService, IOptions<PaymentSettings> paymentSettings)
        {
            _paymentService = paymentService;
            _webhookSecret = paymentSettings.Value.StripeWebhookSecret;
        }

        // POST /stripe-webhook (مسار عام لا يتطلب ترخيص)
        [HttpPost]
        [Consumes("application/json")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _webhookSecret
                );

                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;

                    if (!string.IsNullOrEmpty(session.PaymentIntentId))
                    {
                        var success = await _paymentService
                            .HandlePaymentSuccessAsync(session.PaymentIntentId);

                        if (success)
                        {
                            Console.WriteLine(
                                $"Payment succeeded. PaymentIntentId: {session.PaymentIntentId}"
                            );
                        }
                    }
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}