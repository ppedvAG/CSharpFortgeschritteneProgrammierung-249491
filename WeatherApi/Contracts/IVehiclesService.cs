using LinqSamples.Data;

namespace WeatherApi.Contracts
{
    public interface IVehiclesService
    {
        void Create(Car car);
        bool Delete(Guid id);
        IEnumerable<Car> GetAll();
        Car? GetById(Guid id);
        bool Update(Guid id, Car car);
    }
}