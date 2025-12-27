using car_marketplace_api_3tier.Api.Models;
using car_marketplace_api_3tier.Models;
using Microsoft.EntityFrameworkCore;

namespace car_marketplace_api_3tier.Data
{
    public class CarContext : DbContext
    {
        public CarContext(DbContextOptions<CarContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<User> Users { get; set; } // إضافة DbSet للمستخدمين
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<Order>()
             .Property(o => o.PaymentIntentId)
             .IsRequired(false);

            // Seed Data
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = 1,
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = 2020,
                    Price = 15000,
                    Location = "Riyadh",
                    ImageUrl = "https://example.com/toyota-corolla.jpg",
                    Description = "A reliable and fuel-efficient sedan."
                },
                new Car
                {
                    Id = 2,
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2019,
                    Price = 14000,
                    Location = "Jeddah",
                    ImageUrl = "https://example.com/honda-civic.jpg",
                    Description = "A sporty compact car with great handling."
                },
                new Car
                {
                    Id = 3,
                    Make = "Ford",
                    Model = "Mustang",
                    Year = 2021,
                    Price = 30000,
                    Location = "Dammam",
                    ImageUrl = "https://example.com/ford-mustang.jpg",
                    Description = "A classic American muscle car."
                }

            );
            modelBuilder.Entity<User>().HasData(
         new User { Id = 1, PhoneNumber = "newowner", PasswordHash = "456", Role = "Owner" }, // اسم جديد وكلمة مرور جديدة
         new User { Id = 2, PhoneNumber = "newcustomer", PasswordHash = "789", Role = "Customer" } // اسم جديد وكلمة مرور جديدة
             );
        }
    }
}
