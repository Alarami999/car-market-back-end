using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Data;
using car_marketplace_api_3tier.Models;
using Microsoft.EntityFrameworkCore;

namespace car_marketplace_api_3tier.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly CarContext _context;

        public UserRepository(CarContext context)
        {
            _context = context;
        }

        public async Task<User> GetUserByPhoneNumberAsync(string PhoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == PhoneNumber);
        }

        public async Task<User> AddUserAsync(User newUser)
        {
            await _context.Users.AddAsync(newUser);
            await _context.SaveChangesAsync();
            return newUser;
        }

        //  إضافة Refresh Token جديد
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        //  جلب Refresh Token بواسطة قيمته
        public async Task<RefreshToken> GetRefreshTokenByTokenAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User) 
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        //  تحديث Refresh Token
        public async Task UpdateRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();
        }

        //  جلب مستخدم بواسطة Id
        public async Task<User> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == userId);
        }

        // ---------------------------------------------
        // تنفيذ دوال OTP
        // ---------------------------------------------

        public async Task SaveOtpCodeAsync(OtpCode otpCode)
        {
            
            var existing = await _context.OtpCodes
                                         .FirstOrDefaultAsync(o => o.PhoneNumber == otpCode.PhoneNumber);

            if (existing != null)
            {
                _context.OtpCodes.Remove(existing);
            }

            await _context.OtpCodes.AddAsync(otpCode);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // هذا يلتقط الأخطاء المتعلقة بقاعدة البيانات والقيود
              
                Console.WriteLine(ex.InnerException.Message);
                throw; 
            }
        }

        public async Task<OtpCode> GetOtpCodeAsync(string phoneNumber)
        {
            return await _context.OtpCodes
                                 .FirstOrDefaultAsync(o => o.PhoneNumber == phoneNumber);
        }

        public async Task DeleteOtpCodeAsync(OtpCode otpCode)
        {
            _context.OtpCodes.Remove(otpCode);
            await _context.SaveChangesAsync();
        }

    }
}
