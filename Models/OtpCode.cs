namespace car_marketplace_api_3tier.Api.Models
{
    public class OtpCode
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Code { get; set; } // الرمز المكون من 4-6 أرقام
        public DateTime ExpirationTime { get; set; } // متى ينتهي الرمز
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
