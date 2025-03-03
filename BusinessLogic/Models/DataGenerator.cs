using Bogus;
using System.Drawing;

namespace BusinessLogic.Data
{
    public class DataGenerator
    {
        public const int ReservedSystemColorNameCount = 28;
        public static readonly int ColorCount = (int)KnownColor.YellowGreen;

        public static IEnumerable<Car> GenerateVehicles(int count = 100)
        {
            var brandFaker = new Faker<Brand>()
                .UseSeed(42) // Es soll immer der selbe Output generiert werden
                .RuleFor(m => m.Id, f => f.Random.Guid().ToString())
                .RuleFor(m => m.Name, f => f.Vehicle.Manufacturer())
                .RuleFor(m => m.Country, f => f.Address.Country());

            var carFaker = new Faker<Car>()
                .UseSeed(42)
                .RuleFor(m => m.Id, f => f.Random.Guid().ToString())
                .RuleFor(m => m.Manufacturer, f => brandFaker.Generate())
                .RuleFor(m => m.Model, f => f.Vehicle.Model())
                .RuleFor(m => m.Type, f => f.Vehicle.Type())
                .RuleFor(m => m.Fuel, f => f.Vehicle.Fuel())
                .RuleFor(m => m.TopSpeed, f => f.Random.Number(10, 25) * 10)
                .RuleFor(m => m.Color, f => (KnownColor)f.Random.Number(ReservedSystemColorNameCount, ColorCount));

            return carFaker.Generate(count);
        }
    }
}
