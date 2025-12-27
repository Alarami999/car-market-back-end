// CartService.cs
using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Repositories;

namespace car_marketplace_api_3tier.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICarRepository _carRepository;

        public CartService(ICartRepository cartRepository, ICarRepository carRepository)
        {
            _cartRepository = cartRepository;
            _carRepository = carRepository;
        }

        public async Task<List<CartItem>> GetCartAsync(string userId)
        {
            return await _cartRepository.GetCartItemsByUserIdAsync(userId);
        }

        public async Task<CartItem> AddCarToCartAsync(string userId, int carId, int quantity = 1)
        {
            // التحقق من وجود السيارة أولاً
            var car = await _carRepository.GetCarByIdAsync(carId);
            if (car == null)
            {
                return null; // فشل: السيارة غير موجودة
            }

            var cartItem = new CartItem
            {
                UserId = userId,
                CarId = carId,
                Quantity = quantity
            };

            return await _cartRepository.AddItemToCartAsync(cartItem);
        }

        public async Task<bool> RemoveCarFromCartAsync(string userId, int carId)
        {
            var cartItem = await _cartRepository.GetCartItemByCarIdAsync(userId, carId);

            if (cartItem == null)
            {
                return false; // فشل: العنصر غير موجود في السلة
            }

            return await _cartRepository.RemoveItemFromCartAsync(cartItem);
        }

        public async Task<bool> ClearUserCartAsync(string userId)
        {
            return await _cartRepository.ClearCartAsync(userId);
        }
    }
}