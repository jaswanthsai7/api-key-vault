using Microsoft.AspNetCore.Mvc;
namespace APIVault.API.Controllers.Dummy
{


    [ApiController]
    [Route("api/weather")]
    public class WeatherApiController : ControllerBase
    {
        [HttpGet("GetWeather")]
        public IActionResult GetWeather()
        {
            return Ok(new { status = "Success", data = "Sunny, 28°C" });
        }

        [HttpPost("PostWeather")]
        public IActionResult PostWeather([FromBody] object payload)
        {
            return Ok(new { status = "Success", message = "Weather posted" });
        }
    }


}
