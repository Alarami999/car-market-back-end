using car_marketplace_api_3tier.Api.Models;
using Twilio.Rest.Api.V2010.Account;
using Microsoft.Extensions.Options;
using Twilio;

namespace car_marketplace_api_3tier.Api.Services
{
    public class TwilioSmsService : ISmsService
    {
        private readonly TwilioSettings _settings;

        public TwilioSmsService(IOptions<TwilioSettings> settings)
        {
            _settings = settings.Value;

            // التهيئة
            TwilioClient.Init(_settings.AccountSid, _settings.AuthToken);
        }

        public async Task<bool> SendOtpAsync(string phoneNumber, string otpCode)
        {
            try
            {
                var message = await MessageResource.CreateAsync(
                    body: $"Your verification code is: {otpCode}",
                    from: new Twilio.Types.PhoneNumber(_settings.FromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(phoneNumber)
                );

                return message.ErrorCode == null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Twilio Error: " + ex.Message);
                return false;
            }
        }
    }
}
