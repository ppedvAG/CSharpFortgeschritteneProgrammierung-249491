using WeatherApi.Contracts;
using WeatherApi.Models;

namespace WeatherApi.Services
{
    public class WeatherForecastService : IWeatherForecastService
    {
        private readonly ISummaryService summaryService;

        public WeatherForecastService(ISummaryService summaryService)
        {
            this.summaryService = summaryService;
        }

        // Wir verwenden Task weil theoretisch Daten von einer DB geladen werden können
        public Task<WeatherForecastDto[]> GetAll()
        {
            var summaries = summaryService.GetAll();

            var array = Enumerable.Range(1, 5).Select(index => new WeatherForecastDto
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }).ToArray();

            return Task.FromResult(array);
        }
    }
}
