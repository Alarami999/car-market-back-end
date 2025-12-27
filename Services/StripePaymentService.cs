// StripePaymentService.cs
using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Data;
using car_marketplace_api_3tier.Settings;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using Microsoft.EntityFrameworkCore; // تأكد من استيراد هذه المكتبة

namespace car_marketplace_api_3tier.Services
{
    public class StripePaymentService : IPaymentService
    {
        private readonly PaymentSettings _paymentSettings;
        private readonly CarContext _context;

        public StripePaymentService(IOptions<PaymentSettings> paymentSettings, CarContext context)
        {
            _paymentSettings = paymentSettings.Value;
            _context = context;

            // تعيين مفتاح API السري لـ Stripe
            StripeConfiguration.ApiKey = _paymentSettings.StripeSecretKey;
        }

        public async Task<Session> CreateCheckoutSessionAsync(Order order, string domain)
        {
            // Stripe يتطلب المبلغ بالوحدة الأصغر (هللة / سنت)
            long totalAmountInCents = (long)(order.TotalAmount * 100);

            var lineItems = order.OrderItems.Select(item => new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = _paymentSettings.Currency,
                    UnitAmount = (long)(item.UnitPrice * 100),
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = item.Car.Model,
                    },
                },
                Quantity = item.Quantity,
            }).ToList();

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },

                LineItems = lineItems,

                Mode = "payment",

                SuccessUrl = $"{domain}/payment/success?session_id={{CHECKOUT_SESSION_ID}}",
                CancelUrl = $"{domain}/payment/cancel",

                // 🔴 هذا مهم فقط للرجوع للفرونت
                ClientReferenceId = order.Id.ToString(),

                // ✅ هذا هو التعديل الأهم
                PaymentIntentData = new SessionPaymentIntentDataOptions
                {
                    Metadata = new Dictionary<string, string>
            {
                { "order_id", order.Id.ToString() }
            }
                }
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return session;
        }


        public async Task<bool> HandlePaymentSuccessAsync(string paymentIntentId)
        {
            var service = new PaymentIntentService();
            var intent = await service.GetAsync(paymentIntentId);

            // استخدام ClientReferenceId الذي قمنا بتمريره
            if (int.TryParse(intent.Metadata.GetValueOrDefault("order_id"), out int orderId))
            {
                var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null && order.Status == "Pending")
                {
                    // تحديث حالة الطلب
                    order.Status = "Paid";
                    order.PaymentIntentId = paymentIntentId;
                    await _context.SaveChangesAsync();
                    return true;
                }
            }

            return false;
        }
    }
}