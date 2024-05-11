using CwiczeniaJa.Model;

namespace CwiczeniaJa.Repositories;

public interface ICarRepository
{
    public Task<int> AddCar(Car car);

    public Task<Car> GetCar(int id);
    
    public Task<int> UpdateCar(CarDataTransferObject carDto);
}