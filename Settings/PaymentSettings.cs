// PaymentSettings.cs
namespace car_marketplace_api_3tier.Settings
{
    // يجب أن تتطابق أسماء الخصائص مع مفاتيح قسم "PaymentSettings" في appsettings.json
    public class PaymentSettings
    {
        public string StripeSecretKey { get; set; }
        public string StripePublishableKey { get; set; }
        public string StripeWebhookSecret { get; set; }
        public string Currency { get; set; } = "SAR";
    }
}