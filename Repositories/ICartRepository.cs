// ICartRepository.cs
using car_marketplace_api_3tier.Api.Models;

namespace car_marketplace_api_3tier.Repositories
{
    public interface ICartRepository
    {
        Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId);
        Task<CartItem> GetCartItemByCarIdAsync(string userId, int carId);
        Task<CartItem> AddItemToCartAsync(CartItem item);
        Task<bool> RemoveItemFromCartAsync(CartItem item);
        Task<bool> ClearCartAsync(string userId);
        Task<Order> CreateOrderFromCartAsync(string userId, string shippingAddress);
    }
}