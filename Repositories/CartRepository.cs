// CartRepository.cs
using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Data;
using Microsoft.EntityFrameworkCore;

namespace car_marketplace_api_3tier.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly CarContext _context;

        public CartRepository(CarContext context)
        {
            _context = context;
        }

        public async Task<List<CartItem>> GetCartItemsByUserIdAsync(string userId)
        {
            return await _context.CartItems
                                 .Include(ci => ci.Car) // لجلب بيانات السيارة مع العنصر
                                 .Where(ci => ci.UserId == userId)
                                 .ToListAsync();
        }

        public async Task<CartItem> GetCartItemByCarIdAsync(string userId, int carId)
        {
            return await _context.CartItems
                                 .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.CarId == carId);
        }

        public async Task<CartItem> AddItemToCartAsync(CartItem item)
        {
            var existingItem = await GetCartItemByCarIdAsync(item.UserId, item.CarId);

            if (existingItem != null)
            {
                // إذا كان العنصر موجوداً بالفعل، قم فقط بزيادة الكمية
                existingItem.Quantity += item.Quantity;
                _context.CartItems.Update(existingItem);
                await _context.SaveChangesAsync();
                return existingItem;
            }
            else
            {
                // إذا كان العنصر جديداً
                _context.CartItems.Add(item);
                await _context.SaveChangesAsync();
                return item;
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(CartItem item)
        {
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ClearCartAsync(string userId)
        {
            var items = await GetCartItemsByUserIdAsync(userId);
            _context.CartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Order> CreateOrderFromCartAsync(string userId, string shippingAddress)
        {
            var cartItems = await GetCartItemsByUserIdAsync(userId);
            if (!cartItems.Any()) return null;

            // 1. إنشاء نموذج الطلب (Order)
            var order = new Order
            {
                UserId = userId,
                TotalAmount = cartItems.Sum(ci => ci.Car.Price * ci.Quantity),
                ShippingAddress = shippingAddress,
                Status = "Pending"
            };

            // 2. إنشاء تفاصيل الطلب (OrderItems)
            order.OrderItems = cartItems.Select(ci => new OrderItem
            {
                CarId = ci.CarId,
                Quantity = ci.Quantity,
                UnitPrice = ci.Car.Price,
            }).ToList();

            _context.Orders.Add(order);

            // 3. مسح سلة المشتريات
            _context.CartItems.RemoveRange(cartItems);

            await _context.SaveChangesAsync();

            return order;
        }
    }
}