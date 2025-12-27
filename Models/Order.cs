// Order.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_marketplace_api_3tier.Api.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // مفتاح المستخدم الذي قام بالطلب

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal TotalAmount { get; set; }

        public string Status { get; set; } = "Pending"; // حالات: Pending, Paid, Shipped, Delivered, Canceled

        // معلومات الدفع
        public string? PaymentIntentId { get; set; } // لمعرف عملية الدفع الخارجي (مثل Stripe/PayPal)
        public string ShippingAddress { get; set; } // يمكن توسيعه ليكون نموذجاً خاصاً

        // قائمة بالسيارات في هذا الطلب
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}