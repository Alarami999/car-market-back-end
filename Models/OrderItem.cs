// OrderItem.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_marketplace_api_3tier.Api.Models
{
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } // مفتاح الطلب الذي ينتمي إليه

        [Required]
        public int CarId { get; set; } // مفتاح السيارة

        public int Quantity { get; set; } = 1;

        [Column(TypeName = "decimal(18, 2)")]
        public decimal UnitPrice { get; set; } // سعر السيارة وقت إجراء الطلب

        // العلاقة مع الطلب والسيارة
        [ForeignKey("OrderId")]
        public Order Order { get; set; }

        [ForeignKey("CarId")]
        public Car Car { get; set; }
    }
}