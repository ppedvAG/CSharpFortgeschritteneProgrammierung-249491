
using Serialization.Data;
using System.Net.Http.Json;
using System.Text.Json;
using System.Xml.Serialization;
using LegacyJson = Newtonsoft.Json;

namespace Serialization
{
    internal class Program
    {
        public const string RESOURCE_URL = "https://dummyjson.com/";

        static async Task Main(string[] args)
        {
            var page = await FetchData<RecipePage>("recipes");

            Console.WriteLine("\nSystem.Xml.Serialization");
            var xmlResult = XmlSerialization(page, "recipes.xml");
            xmlResult
                .recipes
                .Take(5)
                .ToList()
                .ForEach(r => Console.WriteLine(r.FormatProps()));

            Console.WriteLine("\nSystem.Text.Json");
            var jsonResult = JsonSerialization(page, "recipes.json");
            jsonResult.recipes
                .Skip(5)
                .Take(5)
                .ToList()
                .ForEach(r => Console.WriteLine(r.FormatProps()));


            Console.WriteLine("\nNewtonsoft.Json");
            var legacyResult = LegacyJsonSerialization(page, "recipes-newton.json");
            legacyResult.recipes
                .Skip(10)
                .Take(5)
                .ToList()
                .ForEach(r => Console.WriteLine(r.FormatProps()));

            Console.ReadKey();
        }

        private static T LegacyJsonSerialization<T>(T page, string filename)
        {
            var settings = new LegacyJson.JsonSerializerSettings()
            {
                Formatting = LegacyJson.Formatting.Indented,
                TypeNameHandling = LegacyJson.TypeNameHandling.Objects, // Vererbung ermoeglichen
                ReferenceLoopHandling = LegacyJson.ReferenceLoopHandling.Ignore,
            };
            
            // schreiben
            var json = LegacyJson.JsonConvert.SerializeObject(page, settings);
            File.WriteAllText(filename, json);

            // lesen
            var fileContent = File.ReadAllText(filename);
            var result = LegacyJson.JsonConvert.DeserializeObject<T>(fileContent);
            return result;
        }

        private static T JsonSerialization<T>(T items, string filename)
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true
            };
            // schreiben
            var json = JsonSerializer.Serialize(items, options);
            File.WriteAllText(filename, json);

            // lesen
            var fileContent = File.ReadAllText(filename);
            var result = JsonSerializer.Deserialize<T>(fileContent);
            return result;
        }

        private static T? XmlSerialization<T>(T items, string filename)
        {
            var serializer = new XmlSerializer(typeof(T));

            // schreiben
            using (var stream = File.Create(filename))
            {
                serializer.Serialize(stream, items);
            }

            // lesen
            using (var reader = new StreamReader(filename))
            {
                return (T)serializer.Deserialize(reader);
            }
        }

        private static async Task<TResult?> FetchData<TResult>(string resource)
        {
            using var client = new HttpClient();
            client.BaseAddress = new Uri(RESOURCE_URL);

            var response = await client.GetAsync(resource);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<TResult>();
                return result;
            }

            return default;
        }
    }
}
