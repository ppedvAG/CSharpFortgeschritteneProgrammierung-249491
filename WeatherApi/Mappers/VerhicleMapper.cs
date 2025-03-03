using LinqSamples.Data;
using WeatherApi.Models;

namespace WeatherApi.Mappers
{
    public static class VerhicleMapper
    {
        public static Car ToEntity(this VehicleDto dto)
        {
            var manufacturer = new Brand 
            { 
                Id = Guid.NewGuid().ToString(),
                Name = dto.Manufacturer 
            };

            return new Car
            {
                Id = dto.Id,
                Color = dto.Color,
                Model = dto.Model,
                Manufacturer = manufacturer,
                Fuel = dto.Fuel,
                TopSpeed = dto.TopSpeed,
                Type = dto.Type
            };
        }

        public static VehicleDto ToDto(this Car entity)
        {
            return new VehicleDto
            {
                Id = entity.Id,
                Color = entity.Color,
                Model = entity.Model,
                Manufacturer = entity.Manufacturer.Name,
                Fuel = entity.Fuel,
                TopSpeed = entity.TopSpeed,
                Type = entity.Type
            };
        }
    }
}
