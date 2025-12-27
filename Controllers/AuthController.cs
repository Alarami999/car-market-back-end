using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Models;
using car_marketplace_api_3tier.Services;
using Microsoft.AspNetCore.Mvc;


namespace car_marketplace_api_3tier.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // *** إضافة جديدة: نقطة نهاية لتجديد الرمز ***
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            if (string.IsNullOrEmpty(request.RefreshToken))
            {
                return BadRequest(new { message = "رمز التحديث مطلوب." });
            }

            var result = await _authService.RefreshTokenAsync(request.RefreshToken);

            if (result == null)
            {
                // رمز التحديث غير صالح أو منتهي
                return Unauthorized(new { message = "رمز التحديث غير صالح أو منتهي الصلاحية. يرجى تسجيل الدخول مجدداً." });
            }

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.PhoneNumber, request.Password);
            if (result == null)
            {
                return Unauthorized();
            }

            return Ok(result);
        }

        // 1. نقطة نهاية لطلب الرمز
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] OtpRequest request)
        {
            // ... التحقق من صحة الإدخال (رقم الهاتف) ...

            var success = await _authService.RequestOtpAsync(request.PhoneNumber);

            if (!success)
                return BadRequest(new { message = "خطأ: رقم الهاتف مسجل بالفعل أو حدث خطأ في إرسال الرمز." });

            return Ok(new { message = "تم إرسال رمز التحقق بنجاح." });
        }

        // 2. نقطة نهاية للتحقق وإتمام التسجيل
        [HttpPost("verify-otp-and-register")]
        public async Task<IActionResult> VerifyOtpAndRegister([FromBody] VerifyAndRegisterRequest request)
        {
            // ... التحقق من صحة الإدخال ...

            var result = await _authService.VerifyOtpAndRegisterAsync(request.PhoneNumber, request.Password, request.OtpCode);

            if (result == null)
                return Unauthorized(new { message = "رمز التحقق غير صالح، منتهي الصلاحية، أو تم تسجيل الدخول مسبقاً." });

            return Ok(result);
        }
    }
}
