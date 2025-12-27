using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Api.Services;
using car_marketplace_api_3tier.Models;
using car_marketplace_api_3tier.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace car_marketplace_api_3tier.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService; // *** إضافة حقل جديد ***

        public AuthService(IUserRepository userRepository, IConfiguration configuration, ISmsService smsService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _smsService = smsService; // *** تعيين الخدمة ***
        }

        // ---------------------------------------------
        // الدالة المساعدة لتوليد الرمز
        // ---------------------------------------------
        private string GenerateRandomOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString(); // رمز مكون من 6 أرقام
        }

        // ---------------------------------------------
        // 1. طلب رمز التحقق (Request OTP)
        // ---------------------------------------------
        public async Task<bool> RequestOtpAsync(string phoneNumber)
        {
            // 1. التحقق من أن رقم الهاتف غير مسجل مسبقاً
            var existingUser = await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
            if (existingUser != null)
            {
                return false; // المستخدم مسجل بالفعل
            }

            // 2. توليد رمز جديد
            var otpCode = GenerateRandomOtp();
            var expirationTime = DateTime.UtcNow.AddMinutes(5); // صلاحية 5 دقائق

            // 3. تخزين الرمز في قاعدة البيانات
            var otpEntity = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = otpCode,
                ExpirationTime = expirationTime
            };
            
             await _userRepository.SaveOtpCodeAsync(otpEntity);

            // 4. إرسال الرمز عبر SMS
            var smsSuccess = await _smsService.SendOtpAsync(phoneNumber, otpCode);

            return smsSuccess;
        }

        // ---------------------------------------------
        // 2. التحقق من الرمز وإتمام التسجيل (Verify OTP and Register)
        // ---------------------------------------------
        public async Task<object> VerifyOtpAndRegisterAsync(string phoneNumber, string password, string otpCode)
        {
            // 1. جلب رمز OTP من قاعدة البيانات
            var storedOtp = await _userRepository.GetOtpCodeAsync(phoneNumber);

            // 2. التحقق من صحة الرمز
            if (storedOtp == null || storedOtp.Code != otpCode || storedOtp.ExpirationTime < DateTime.UtcNow)
            {
                return null; // رمز غير صالح أو منتهي
            }

            // 3. الرمز صحيح... نبدأ عملية إنشاء المستخدم

            // إنشاء هاش لكلمة المرور
            var hasher = new PasswordHasher<User>();
            var user = new User
            {
                PhoneNumber = phoneNumber,
                Role = "Customer"
            };

            // تشفير كلمة المرور
            user.PasswordHash = hasher.HashPassword(user, password);

            // حفظ المستخدم في قاعدة البيانات
            await _userRepository.AddUserAsync(user);

            // 4. حذف رمز OTP بعد الاستخدام
            await _userRepository.DeleteOtpCodeAsync(storedOtp);

            // 5. إنشاء JWT Access Token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            // 6. إنشاء Refresh Token
            var refreshToken = GenerateRefreshToken(user.Id);
            await _userRepository.AddRefreshTokenAsync(refreshToken);

            // 7. إرجاع النتيجة (مثل LoginAsync تماماً)
            return new
            {
                accessToken = accessTokenString,
                refreshToken = refreshToken.Token,
                user = new { PhoneNumber = user.PhoneNumber, role = user.Role }
            };
        }

        // ---------------------------------------------
        // وظيفة إنشاء رمز التحديث
        // ---------------------------------------------
        private RefreshToken GenerateRefreshToken(int userId)
        {
            var randomNumber = new byte[32];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomNumber),
                    Expires = DateTime.UtcNow.AddDays(7), // صلاحية طويلة
                    Created = DateTime.UtcNow,
                    UserId = userId
                };
            }
        }

        // ---------------------------------------------
        // وظيفة تجديد الرمز (RefreshTokenAsync)
        // ---------------------------------------------
        public async Task<object> RefreshTokenAsync(string oldRefreshToken)
        {
            var refreshTokenEntity = await _userRepository.GetRefreshTokenByTokenAsync(oldRefreshToken);

            // التحقق من صلاحية الرمز
            if (refreshTokenEntity == null || refreshTokenEntity.IsRevoked || refreshTokenEntity.Expires < DateTime.UtcNow)
                return null; // فشل التجديد

            // إبطال رمز التحديث القديم
            refreshTokenEntity.IsRevoked = true;
            await _userRepository.UpdateRefreshTokenAsync(refreshTokenEntity);

            // الحصول على المستخدم
            var user = await _userRepository.GetUserByIdAsync(refreshTokenEntity.UserId);

            // إنشاء Access Token جديد
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.PhoneNumber),
            new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            // إنشاء Refresh Token جديد
            var newRefreshToken = GenerateRefreshToken(user.Id);
            await _userRepository.AddRefreshTokenAsync(newRefreshToken);

            // إرجاع الرموز الجديدة
            return new
            {
                accessToken = accessTokenString,
                refreshToken = newRefreshToken.Token,
                user = new { PhoneNumber = user.PhoneNumber, role = user.Role }
            };
        }

        public async Task<object> LoginAsync(string PhoneNumber, string password)
        {
            var user = await _userRepository.GetUserByPhoneNumberAsync(PhoneNumber);
            if (user == null)
            {
                return null;
            }

            // 2) إنشاء الهاشر
            var hasher = new PasswordHasher<User>();

            // 3) التحقق من كلمة المرور
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);

            // 4) إذا كلمة المرور خاطئة
            if (result == PasswordVerificationResult.Failed)
            {
                return null;
            }

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.PhoneNumber),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var accessToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            var refreshToken = GenerateRefreshToken(user.Id);
    await _userRepository.AddRefreshTokenAsync(refreshToken);

    // 4. إرجاع رمز الوصول ورمز التحديث
    return new 
    { 
        accessToken = accessTokenString, 
        refreshToken = refreshToken.Token, 
        user = new { PhoneNumber = user.PhoneNumber, role = user.Role } 
    };
        }

        
    }
}
