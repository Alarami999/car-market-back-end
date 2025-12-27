namespace car_marketplace_api_3tier.Api.Models
{
    public class VerifyAndRegisterRequest
    {
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string OtpCode { get; set; }
    }
}
