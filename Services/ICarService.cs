namespace car_marketplace_api_3tier.Services
{
    public interface ICarService
    {
        Task<IEnumerable<Car>> GetCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<Car> AddNewCarAsync(Car newCar);
        Task<Car> UpdateExistingCarAsync(Car updatedCar);
        Task<bool> DeleteCarByIdAsync(int id);
    }
}
