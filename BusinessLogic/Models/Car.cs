using System.Drawing;

namespace BusinessLogic.Data
{
    public class Car
    {
        public string Id { get; set; }

        public Brand Manufacturer { get; set; }

        public string Model { get; set; }

        public string Type { get; set; }

        public string Fuel { get; set; }

        public int TopSpeed { get; set; }

        public KnownColor Color { get; set; }

        public override string ToString()
        {
            return $"{Color,-18} {Manufacturer,-18} {Model,-18} {Type,-18} {Fuel,-10} {TopSpeed} km/h ";
        }
    }
}
