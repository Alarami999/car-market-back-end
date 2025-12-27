namespace car_marketplace_api_3tier.Services
{
public interface IAuthService
{
    // LoginAsync الآن يرجع object يحتوي على Access Token و Refresh Token
    Task<object> LoginAsync(string PhoneNumber, string password);

     // *** إضافة جديدة: وظائف الـ OTP ***
     Task<bool> RequestOtpAsync(string phoneNumber);
     Task<object> VerifyOtpAndRegisterAsync(string phoneNumber, string password, string otpCode);

     // إضافة دالة تجديد رمز التحديث
     Task<object> RefreshTokenAsync(string oldRefreshToken);
}

}