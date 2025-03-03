
using BusinessLogic.Contracts;
using BusinessLogic.Services;
using System.Text.Json.Serialization;
using WeatherApi.Contracts;
using WeatherApi.Services;

namespace WeatherApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            RegisterDependencyTypes(builder);

            StartUp(builder);
        }

        private static void RegisterDependencyTypes(WebApplicationBuilder builder)
        {
            // Add services to the container.

            // Singleton gilt fuer die gesammte Lebenszeit der laufenden Anwendung
            builder.Services.AddSingleton<ISummaryService, SummaryService>();

            // Scoped wird eine Instanz erzeugt, die fuer den gesammten Request gilt
            builder.Services.AddScoped<IWeatherForecastService, WeatherForecastService>();

            // Bei Transient wird beim aufloesen der Dependency jedes mal eine neue Instanz erzeugt
            builder.Services.AddTransient<IWeatherForecastService, WeatherForecastService>();

            builder.Services.AddSingleton<IVehiclesService, VehiclesService>();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                // Der JsonStringEnumConverter konvertiert Strings in Enum-Werte und zurueck statt integers auszugeben
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
        }

        private static void StartUp(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
