// CartItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_marketplace_api_3tier.Api.Models
{
    public class CartItem
    {
        [Key]
        public int Id { get; set; }

        // يمثل مفتاح المستخدم الذي أضاف السيارة للسلة (من جدول AspNetUsers)
        [Required]
        public string UserId { get; set; }

        // مفتاح السيارة المضافة
        [Required]
        public int CarId { get; set; }

        // الكمية (افتراضياً 1 للسيارة الواحدة)
        public int Quantity { get; set; } = 1;

        // العلاقة بين العناصر
        [ForeignKey("CarId")]
        public Car Car { get; set; }

        // لن تحتاج إلى User object هنا غالباً، لكن العلاقة موجودة في قاعدة البيانات
    }
}