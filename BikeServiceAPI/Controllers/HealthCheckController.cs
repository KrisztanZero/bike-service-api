using Microsoft.AspNetCore.Mvc;

namespace BikeServiceAPI.Controllers;

[ApiController]
[Route("[Controller]")]
public class HealthCheckController : Controller
{
    /// <summary>
    /// Checks the health of the application.
    /// </summary>
    /// <returns>The health status of the application.</returns>
    [HttpGet]
    public IActionResult CheckHealth()
    {
        var status = new { Status = "Online" };
        return Ok(status);
    }
}