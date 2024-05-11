using CwiczeniaJa.Model;
using CwiczeniaJa.Services;
using Microsoft.AspNetCore.Mvc;

namespace CwiczeniaJa.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarController : ControllerBase
{
    private ICarService _carService;

    public CarController(ICarService carService)
    {
        _carService = carService;
    }

    [HttpPut("AddCar")]
    public async Task<IActionResult> AddCar(Car car)
    {
        var res = await _carService.AddCar(car);

        if (res == (int)CarErrorCodes.CarNotAdded)
        {
            return Conflict("Car has not been added, check if the provided data is valid!");
        }

        return Ok("Added a new car, its ID = " + res);
    }

    [HttpGet("GetCar")]
    public async Task<IActionResult> GetCar(int id)
    {
        var res = await _carService.GetCar(id);

        if (res == null)
        {
            return NotFound("The provided Id does not correspond to any object in the database!");
        }

        return Ok(res);
    }

    [HttpPost("UpdateCar")]
    public async Task<IActionResult> UpdateCar(CarDataTransferObject carDto)
    {
        var res = await _carService.UpdateCar(carDto);

        if (res == (int)CarErrorCodes.CarDoesNotExistInDb)
        {
            return NotFound("Old car was not found in the database!");
        }

        if (res == (int)CarErrorCodes.CarUpdateProblem)
        {
            return NotFound("Cannot update the old car, contact the administrator!");
        }

        return StatusCode(StatusCodes.Status201Created);
    }
}

public enum CarErrorCodes
{
    CarNotAdded = -1,
    OldCarIsNull = -2,
    NewCarIsNull = -3,
    CarDoesNotExistInDb = -4,
    CarUpdateProblem = -5,
}