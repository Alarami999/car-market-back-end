namespace car_marketplace_api_3tier.Api.Services
{
    public interface ISmsService
    {
        Task<bool> SendOtpAsync(string phoneNumber, string otpCode);
    }
}
