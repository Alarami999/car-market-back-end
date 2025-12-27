using car_marketplace_api_3tier.Repositories;

namespace car_marketplace_api_3tier.Services
{
    public class CarService : ICarService
    {
        private readonly ICarRepository _carRepository;

        public CarService(ICarRepository carRepository)
        {
            _carRepository = carRepository;
        }

        public async Task<IEnumerable<Car>> GetCarsAsync()
        {
            return await _carRepository.GetAllCarsAsync();
        }

        public async Task<Car> GetCarByIdAsync(int id)
        {
            return await _carRepository.GetCarByIdAsync(id);
        }

        public async Task<Car> AddNewCarAsync(Car newCar)
        {
            return await _carRepository.AddCarAsync(newCar);
        }

        public async Task<Car> UpdateExistingCarAsync(Car updatedCar)
        {
            return await _carRepository.UpdateCarAsync(updatedCar);
        }

        public async Task<bool> DeleteCarByIdAsync(int id)
        {
            return await _carRepository.DeleteCarAsync(id);
        }
    }
}
