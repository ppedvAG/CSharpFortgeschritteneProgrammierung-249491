using LinqSamples.Data;
using System.Drawing;

namespace WeatherApi.Models
{
    public class VehicleDto
    {
        public string? Id { get; set; }

        public string? Manufacturer { get; set; }

        public string Model { get; set; }

        public string? Type { get; set; }

        public string? Fuel { get; set; }

        public int TopSpeed { get; set; }

        public KnownColor Color { get; set; }
    }
}
