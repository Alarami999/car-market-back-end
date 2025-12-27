using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Models;

namespace car_marketplace_api_3tier.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetUserByPhoneNumberAsync(string PhoneNumber);
        Task<User> AddUserAsync(User newUser);

        // *** وظائف الـ Refresh Token الجديدة ***
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task<RefreshToken> GetRefreshTokenByTokenAsync(string token);
        Task UpdateRefreshTokenAsync(RefreshToken refreshToken);
        Task<User> GetUserByIdAsync(int userId);

        //وظائف OtpCode
        Task SaveOtpCodeAsync(OtpCode otpCode);
        Task<OtpCode> GetOtpCodeAsync(string phoneNumber);
        Task DeleteOtpCodeAsync(OtpCode otpCode);
    }
}
