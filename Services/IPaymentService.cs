// IPaymentService.cs
using car_marketplace_api_3tier.Api.Models;
using Stripe.Checkout;

namespace car_marketplace_api_3tier.Services
{
    public interface IPaymentService
    {
        // لإنشاء جلسة دفع Stripe جديدة
        Task<Session> CreateCheckoutSessionAsync(Order order, string domain);

        // لمعالجة الويب هوك الخاص بـ Stripe (تأكيد الدفع)
        Task<bool> HandlePaymentSuccessAsync(string paymentIntentId);
    }
}