using car_marketplace_api_3tier.Models;

namespace car_marketplace_api_3tier.Api.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public string Token { get; set; } // رمز التحديث الفعلي
        public int UserId { get; set; } // مفتاح خارجي لربط المستخدم
        public DateTime Expires { get; set; } // متى ينتهي الرمز (عادةً 7 أيام أو أكثر)
        public DateTime Created { get; set; }
        public bool IsRevoked { get; set; } = false; // هل تم إبطاله يدوياً

        // Navigation Property
        public User User { get; set; }
    
}
}
