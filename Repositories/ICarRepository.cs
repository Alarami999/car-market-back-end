namespace car_marketplace_api_3tier.Repositories
{
    public interface ICarRepository
    {
        Task<IEnumerable<Car>> GetAllCarsAsync();
        Task<Car> GetCarByIdAsync(int id);
        Task<Car> AddCarAsync(Car newCar);
        Task<Car> UpdateCarAsync(Car updatedCar);
        Task<bool> DeleteCarAsync(int id);
    }
}
