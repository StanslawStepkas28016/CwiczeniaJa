using System.Data.SqlClient;
using CwiczeniaJa.Controllers;
using CwiczeniaJa.Model;

namespace CwiczeniaJa.Repositories;

public class CarRepository : ICarRepository
{
    private readonly string? _connectionString;

    public CarRepository(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

    public async Task<int> AddCar(Car car)
    {
        const string insertCarQuery =
            "INSERT INTO Car VALUES (@Brand ,@Price, ); " +
            "SELECT SCOPE_IDENTITY();";

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var transaction = connection.BeginTransaction();
        await using var insertCommand = new SqlCommand(insertCarQuery, connection, transaction);


        insertCommand.Parameters.AddWithValue("@Brand", car.Brand);
        insertCommand.Parameters.AddWithValue("@Price", car.Price);
        insertCommand.Parameters.AddWithValue("@ManufacturedDate", car.ManufacturedDate);

        try
        {
            var carIdResult = await insertCommand.ExecuteScalarAsync();
            transaction.Commit();
            return Convert.ToInt32(carIdResult);
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync();
            return (int)CarErrorCodes.CarNotAdded;
        }
    }

    public async Task<Car> GetCar(int id)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        await using var command = new SqlCommand("SELECT * FROM Car WHERE Id = @Id;", connection);
        command.Parameters.AddWithValue("@Id", id);

        var reader = await command.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            var data = new Car
            {
                Id = (int)reader["Id"],
                Brand = reader["Brand"].ToString(),
                Price = (decimal)reader["Price"],
                ManufacturedDate = reader["ManufacturedDate"].ToString()
            };

            return data;
        }

        return null;
    }

    public async Task<int> UpdateCar(CarDataTransferObject carDto)
    {
        // Walidacja — czy stary samochód jest null.
        if (carDto.OldCar == null)
        {
            return (int)CarErrorCodes.OldCarIsNull;
        }

        // Walidacja — czy nowy samochód jest null.
        if (carDto.NewCar == null)
        {
            return (int)CarErrorCodes.NewCarIsNull;
        }

        // Walidacja — czy stary samochód istnieje w bazie.
        if (await DoesOldCarExistInDatabase(carDto.OldCar) == false)
        {
            return (int)CarErrorCodes.CarDoesNotExistInDb;
        }

        // Aktualizacja danych w bazie.
        const string query =
            "UPDATE Car SET Brand = @New_Brand, " +
            "               Price = @New_Price," +
            "               ManufacturedDate = @New_Date";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        await connection.OpenAsync(); // kolejność.

        command.Parameters.AddWithValue("@New_Brand", carDto.NewCar.Brand);
        command.Parameters.AddWithValue("@New_Price", carDto.NewCar.Price);
        command.Parameters.AddWithValue("@New_Date", carDto.NewCar.ManufacturedDate);

        var rowsAffected = await command.ExecuteNonQueryAsync();

        if (rowsAffected != 1)
        {
            return (int)CarErrorCodes.CarUpdateProblem;
        }

        return 1;
    }

    private async Task<bool> DoesOldCarExistInDatabase(Car oldCar)
    {
        const string query =
            "SELECT 1 AS DoesOldCarExist FROM Car WHERE Id = @OldCarId;";

        await using var connection = new SqlConnection(_connectionString);
        await using var command = new SqlCommand(query, connection);
        await connection.OpenAsync(); // kolejność.

        command.Parameters.AddWithValue("@OldCarId", oldCar.Id);

        var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            if (reader["DoesOldCarExist"] != DBNull.Value) return true;
        }

        return false;
    }
}