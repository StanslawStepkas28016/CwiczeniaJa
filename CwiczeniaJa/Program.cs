
using CwiczeniaJa.Repositories;
using CwiczeniaJa.Services;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder();

        // Rejestracja serwisów.
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ICarRepository, CarRepository>();
        builder.Services.AddScoped<ICarService, CarService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.MapControllers();

        app.Run();
    }
}