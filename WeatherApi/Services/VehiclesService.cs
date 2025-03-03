using LinqSamples.Data;
using WeatherApi.Contracts;

namespace WeatherApi.Services
{
    // Vehicle Service mit CRUD Operationen
    public class VehiclesService : IVehiclesService
    {
        private readonly List<Car> _data = DataGenerator.GenerateVehicles().ToList();

        public IEnumerable<Car> GetAll() => _data; // IEnumerable<Car>

        public Car? GetById(Guid id) => _data.FirstOrDefault(x => x.Id == $"{id}");

        public string Create(Car car)
        {
            car.Id = Guid.NewGuid().ToString();
            _data.Add(car);
            return car.Id;
        }

        public bool Update(Guid id, Car car)
        {
            var existing = _data.FirstOrDefault(x => x.Id == $"{id}");
            if (existing is null)
            {
                return false;
            }

            var index = _data.IndexOf(existing);
            car.Id = _data[index].Id;
            _data[index] = car;
            return true;
        }

        public bool Delete(Guid id)
        {
            var removedElementCount = _data.RemoveAll(x => x.Id == $"{id}");
            return removedElementCount > 0;
        }
    }
}
