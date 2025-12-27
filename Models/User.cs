using car_marketplace_api_3tier.Api.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace car_marketplace_api_3tier.Models
{
    [Table("Users")]
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } // "Owner" أو "Customer"

        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}

