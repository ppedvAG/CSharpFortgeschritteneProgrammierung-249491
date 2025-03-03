using BusinessLogic.Data;
using LinqSamples.Extensions;
using System.Text;

namespace LinqSamples
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LinqSamples();

            ExtensionMethodSample();

            Console.ReadKey();
        }

        private static void ExtensionMethodSample()
        {
            int number = 4711;
            var sum = ExtensionMethods.DigitSum(number);

            // Als Extension Method
            sum = number.DigitSum();
            Console.WriteLine($"Die Quersumme von {number} ist {sum}");
        }

        private static void LinqSamples()
        {
            var vehicles = DataGenerator.GenerateVehicles(100);

            Console.WriteLine("Top 10 Vehicles");
            // Klassische Ansatz waere...
            //for (int i = 0; i < 10; i++)
            //{
            //    Console.WriteLine($"Vehicle {i}: {vehicles.ToArray()[i]}");
            //}

            // Besser und einfacher ist Linq
            vehicles.Take(10)
                .ToList()
                .ForEach(Console.WriteLine);

            var averageSpeed = vehicles.Take(10).Average(v => v.TopSpeed);
            var maxSpeed = vehicles.Take(10).Max(v => v.TopSpeed);
            var minSpeed = vehicles.Take(10).Min(v => v.TopSpeed);
            Console.WriteLine($"\nAverage Speed: {averageSpeed} km/h, Max Speed: {maxSpeed} km/h, Min Speed: {minSpeed} km/h");

            // Exception wenn Liste leer waere
            Console.WriteLine($"\nFirst vehicle: {vehicles.First()}");

            // Keine Exception sondern default, was bei Car null entspricht
            Console.WriteLine($"Last vehicle: {vehicles.LastOrDefault()}");

            // Zweite "Seite" der Fahrzeugliste
            var secondPage = vehicles.Skip(10).Take(10);
            Console.WriteLine("\nSecond Page:");
            secondPage.ToList().ForEach(Console.WriteLine);

            Console.WriteLine("\n\nAlle Fahrzeuge mit einem gelben Farbton");
            vehicles.Where(v => v.Color.ToString().Contains("Yellow")).ToList().ForEach(Console.WriteLine);

            Console.WriteLine("\n\nFahrzeuge sortieren nach TopSpeed und Model.");
            vehicles.OrderByDescending(v => v.TopSpeed)
                .ThenBy(v => v.Model)
                .Take(10)
                .ToList()
                .ForEach(Console.WriteLine);

            Console.WriteLine("\n\nFahrzeuge nach Fuel gruppieren.");
            IEnumerable<IGrouping<string, Car>> groups = vehicles.GroupBy(v => v.Fuel);
            // Fahrzeuge jeder Gruppe zaehlen wofuer wir ein Anonymes Object temporaer erstellen
            groups.Select(g => new { Fuel = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .ToList()
                .ForEach(g => Console.WriteLine($"{g.Count} Fahrzeuge betrieben mit {g.Fuel}."));

            // local Function statt Func<StringBuilder, Car, StringBuilder>
            static StringBuilder AppendLine(StringBuilder sb, Car v) => sb.AppendLine($"Der {v.Color} {v.Model} faehrt mit {v.TopSpeed} km/h.");

            var sb = vehicles.Take(10).Aggregate(new StringBuilder(), AppendLine);
            Console.WriteLine(sb.ToString());

            var dict = vehicles.Take(20)
                .Select(v => new { Brand = v.Manufacturer.Name, Vehicle = v})
                .GroupBy(v => v.Brand)
                .ToDictionary(g => g.Key, g => g.Select(v => v.Vehicle).Aggregate(new StringBuilder(), AppendLine).ToString());

            // Select wird an dieser Stelle noch nicht evaluiert sondern erst bei ToList() weil IEnumerable "lazy" ist.
            var output = dict.Select(p =>
            {
                Console.WriteLine($"--- {p.Key} ---\n{p.Value}");
                return p;
            });

            Console.WriteLine(@"Es wurde noch nichts in die Console geschrieben. 
Erst ToList() / ToArray() / ToDictionary() evaluiert das Linq Statement.");

            _ = output.ToList();
        }
    }
}
