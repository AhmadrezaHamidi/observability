using Microsoft.AspNetCore.Mvc;

namespace SmapleProjectB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public List<Person> Get()
        {
            var res = new List<Person>();
            for (int i = 0; i < 20; i++)
                res.Add(new Person(i, $"name{i}", i + 10));
            Task.Delay(500);

            return res;
        }
    }
    public class Person
    {
        public Person(int id, string userName, int age)
        {
            Id = id;
            this.userName = userName;
            Age = age;
        }

        public int Id { get; set; }
        public string userName { get; set; }
        public int Age { get; set; }

    }

}
