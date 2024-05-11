using CwiczeniaJa.Model;
using CwiczeniaJa.Repositories;

namespace CwiczeniaJa.Services;

public class CarService : ICarService
{
    private ICarRepository _carRepository;

    public CarService(ICarRepository carRepository)
    {
        _carRepository = carRepository;
    }

    public async Task<int> AddCar(Car car)
    {
        return await _carRepository.AddCar(car);
    }

    public async Task<Car> GetCar(int id)
    {
        return await _carRepository.GetCar(id);
    }

    public async Task<int> UpdateCar(CarDataTransferObject carDto)
    {
        return await _carRepository.UpdateCar(carDto);
    }
}