using car_marketplace_api_3tier.Data;
using Microsoft.EntityFrameworkCore;

namespace car_marketplace_api_3tier.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly CarContext _context;

        public CarRepository(CarContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Car>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _context.Cars.FindAsync(id);
        }

        public async Task<Car> AddCarAsync(Car newCar)
        {
            await _context.Cars.AddAsync(newCar);
            await _context.SaveChangesAsync();
            return newCar;
        }

        public async Task<Car> UpdateCarAsync(Car updatedCar)
        {
            var existingCar = await _context.Cars.FindAsync(updatedCar.Id);
            if (existingCar != null)
            {
                _context.Entry(existingCar).CurrentValues.SetValues(updatedCar);
                await _context.SaveChangesAsync();
            }
            return existingCar;
        }

        public async Task<bool> DeleteCarAsync(int id)
        {
            var carToRemove = await _context.Cars.FindAsync(id);
            if (carToRemove != null)
            {
                _context.Cars.Remove(carToRemove);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}