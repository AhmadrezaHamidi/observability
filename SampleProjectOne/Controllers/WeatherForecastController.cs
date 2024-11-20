using Microsoft.AspNetCore.Mvc;

namespace SampleProjectOne.Controllers
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
        private readonly HttpClient _httpClient;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, HttpClient httpClient)
        {
            _logger = logger;
            _httpClient = httpClient;
        }

        [HttpGet(Name = "GetWeatherForecastFromB")]
        public async Task<IActionResult> GetWeatherForecastMustBeClick()
        {
            var url = "http://localhost:5266/weatherforecast";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode(); // Ensure response is successful

                var content = await response.Content.ReadAsStringAsync(); // Read response as string
                return Content(content, "application/json"); // Return JSON response
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error calling : {ex.Message}");
            }
        }
    }
}
