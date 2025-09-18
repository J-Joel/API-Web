using API_Web.Models.Ejemplo;
using Microsoft.AspNetCore.Mvc;

namespace API_Web.Controllers.Ejemplo
{
    [ApiController]
    [Route("[controller]")]
    [ApiExplorerSettings(GroupName = "Ejemplo")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        //------------------------------------------------ Ejemplo ---------------------------------------------------------------------
        private static readonly string[] Summaries =
        [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];
        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            Console.WriteLine($"Datos enviados");
            // El Enumerable es una especie de interacion, solamente disponible en una clase IEnumerable
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();// Lo que se devuelve es un array de objetos, en este caso de la clase, pero se termina serealizando en JSON
        }
        //------------------------------------------------------------------------------------------------------------------------------
    }
}
