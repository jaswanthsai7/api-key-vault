using Microsoft.AspNetCore.Mvc;

namespace APIVault.API.Controllers.Dummy
{
    [ApiController]
    [Route("api/stock")]
    public class StockApiController : ControllerBase
    {
        [HttpGet("GetStock")]
        public IActionResult GetStock()
        {
            return Ok(new { status = "Success", data = "AAPL: $175" });
        }

        [HttpPost("PostStock")]
        public IActionResult PostStock([FromBody] object payload)
        {
            return Ok(new { status = "Success", message = "Stock posted" });
        }
    }
}
