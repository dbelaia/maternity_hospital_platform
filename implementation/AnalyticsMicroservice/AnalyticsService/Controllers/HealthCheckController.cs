using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AnalyticsService.Repository;

namespace AnalyticsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly IHealthCheckService _healthCheckService;

        public HealthCheckController(IHealthCheckService healthCheckService)
        {
            _healthCheckService = healthCheckService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var isHealthy = await _healthCheckService.CheckHealthAsync();
            if (isHealthy)
            {
                return Ok(new { status = "Healthy" });
            }
            return StatusCode(503, new { status = "Unhealthy" });
        }
    }
}
