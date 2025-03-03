using BusinessLogic.Attributes;
using BusinessLogic.Data;
using BusinessLogic.Services;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace Reflection
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Reflection arbeitet mit Typen
            // d.h. 2 Moeglichkeiten
            var car = DataGenerator.GenerateVehicles(1).First();

            // Vorsicht wenn car null ist gibt es eine NullReferenceException
            var carType = car.GetType();

            // besser:
            carType = typeof(Car);

            string carInfo = PrintProperties(car);
            Console.WriteLine(carInfo);

            // Alle Members, d. h. fields, consturctors, methods, properties ausgeben
            Console.WriteLine(PrintMembers<VehiclesService>());

            // aktuelles Projekt, also "Reflection"
            _ = Assembly.GetExecutingAssembly();

            // BusinessLogic Assembly
            var linqAssembly = Assembly.GetAssembly(typeof(VehiclesService));

            // Alternativ DLL aus bin Verzeichnis laden
            var businessAssembly = LoadAssemblyFromFile("BusinessLogic");

            Console.WriteLine("\n\nAlle Services welche mit SerivceAttribute markiert wurden dynamisch laden.");
            var services = businessAssembly.GetTypes()
                .Where(t => t.GetCustomAttribute<ServiceAttribute>() != null)
                .ToList();

            foreach (var service in services)
            {
                var attr = service.GetCustomAttribute<ServiceAttribute>();
                Console.WriteLine($"{service.Name}.cs\t{attr.Description}");
            }

            // Wir koennen auch auf private Members zugreifen und sogar readonly Felder setzen, obwohl das verboten sein sollte!
            // Der Compiler wuerde hier eine Fehler schmeissen, aber wir umgehen das mit Reflection zur Laufzeit und tun dies doch!
            Console.WriteLine("\n\nPrivate readonly Fields von VehicleService setzen");
            var readonlyDataListOfCars = typeof(VehiclesService).GetFields(BindingFlags.Instance | BindingFlags.NonPublic).First();

            {
                var vehicleService = new VehiclesService();
                readonlyDataListOfCars.SetValue(vehicleService, new List<Car>
                {
                    new Car { Model = "Infiltrated Car!", Type = "Hacked", Color = KnownColor.Red }
                });

                // Inhalte der neuen Liste als Beweis anzeigen
                vehicleService.GetAll()
                    .Select(c => c.ToString())
                    .ToList()
                    .ForEach(Console.WriteLine);
            }

            Console.WriteLine("\n\nPress any key to exit");
            Console.ReadKey();
        }

        private static string PrintProperties<T>(T obj)
        {
            StringBuilder Properties(StringBuilder sb, PropertyInfo p) => sb.Append($"{p.Name}: {p.GetValue(obj)}");
            return typeof(T)
                .GetProperties()
                .Aggregate(new StringBuilder(), Properties)
                .ToString();
        }

        private static string PrintMembers<T>()
        {
            return typeof(T)
                .GetMembers()
                .Aggregate(new StringBuilder(), (sb, m) => sb.AppendLine($"{m.MemberType}\t{m.Name}\t{m.DeclaringType}\t{m.ReflectedType}"))
                .ToString();
        }

        private static Assembly LoadAssemblyFromFile(string assemblyName)
        {
            var path = Environment.CurrentDirectory.Replace(nameof(Reflection), assemblyName);
            var filepath = Path.Combine(path, $"{assemblyName}.dll");
            if (!File.Exists(filepath))
            {
                throw new FileNotFoundException(filepath);
            }

            Console.WriteLine("Assembly laden von " + filepath);
            return Assembly.LoadFrom(filepath);
        }
    }
}
