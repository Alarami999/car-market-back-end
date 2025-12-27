using car_marketplace_api_3tier.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace car_marketplace_api_3tier.Controllers
{
    [ApiController]
    [Route("Config")]
    public class ConfigController : ControllerBase
    {
        private readonly PaymentSettings _paymentSettings;

        public ConfigController(IOptions<PaymentSettings> paymentSettings)
        {
            _paymentSettings = paymentSettings.Value;
        }

        // GET /Config/stripe-key
        [HttpGet("stripe-key")]
        public IActionResult GetStripePublishableKey()
        {
            return Ok(new
            {
                publishableKey = _paymentSettings.StripePublishableKey
            });
        }
    }
}
