// ICartService.cs
using car_marketplace_api_3tier.Api.Models;

namespace car_marketplace_api_3tier.Services
{
    public interface ICartService
    {
        Task<List<CartItem>> GetCartAsync(string userId);
        Task<CartItem> AddCarToCartAsync(string userId, int carId, int quantity = 1);
        Task<bool> RemoveCarFromCartAsync(string userId, int carId);
        Task<bool> ClearUserCartAsync(string userId);
    }
}