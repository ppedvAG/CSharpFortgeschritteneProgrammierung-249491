using WeatherApi.Contracts;

namespace WeatherApi.Services
{
    public class SummaryService : ISummaryService
    {
        public string[] GetAll()
        {
            return
            [
                "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
            ];
        }
    }
}
