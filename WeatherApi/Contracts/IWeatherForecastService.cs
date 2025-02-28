using WeatherApi.Models;

namespace WeatherApi.Contracts
{
    // Normalerweise sollten Contracts in einer Domain-Assembly definiert werden,
    // d. h. in einem zentralen Projekt das von allen moeglichen UI-Projekten verwendet werden kann
    public interface IWeatherForecastService
    {
        Task<WeatherForecastDto[]> GetAll();
    }
}