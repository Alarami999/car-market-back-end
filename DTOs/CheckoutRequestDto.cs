// CheckoutRequestDto.cs
using System.ComponentModel.DataAnnotations;

namespace car_marketplace_api_3tier.DTOs
{
    public class CheckoutRequestDto
    {
        [Required]
        // هذا العنوان سيكون افتراضياً لمعلومات الشحن/الفواتير.
        public string ShippingAddress { get; set; }

        // يمكن إضافة حقول أخرى مثل معلومات العميل أو تفضيلات الدفع.
    }
}