using Microsoft.AspNetCore.Mvc;

namespace SmartShop.API.Controllers
{
    /// <summary>
    /// Provides an endpoint to check the status of the API service.
    /// </summary>
    /// <remarks>
    /// This controller exposes a health check endpoint for monitoring service availability.
    /// The base route for this controller is "api/Status".
    /// </remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        // GET: api/Status
        [HttpGet]
        public IActionResult GetStatus()
        {
            return Ok(new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}