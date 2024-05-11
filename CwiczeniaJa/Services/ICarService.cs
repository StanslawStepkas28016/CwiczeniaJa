using CwiczeniaJa.Model;

namespace CwiczeniaJa.Services;

public interface ICarService
{
    public Task<int> AddCar(Car car);

    public Task<Car> GetCar(int id);

    public Task<int> UpdateCar(CarDataTransferObject carDto);
}