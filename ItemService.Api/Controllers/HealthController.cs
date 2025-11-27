using ItemService.Api.Responses;
using ItemService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ItemService.Api.Controllers
{
    /// <summary>
    /// Health check controller for monitoring service and database status.
    /// </summary>
    [ApiController]
    [Route("/health")]
    public class HealthController : ControllerBase
    {
        private readonly IMongoDbContext _dbContext;
        private readonly IHostApplicationLifetime _lifetime;
        private readonly static DateTime _start = DateTime.UtcNow;

        /// <summary>
        /// Initializes a new instance of the <see cref="HealthController"/> class.
        /// </summary>
        /// <param name="dbContext">MongoDB context for database connectivity check.</param>
        /// <param name="lifetime">Application lifetime for uptime calculation.</param>
        public HealthController(IMongoDbContext dbContext, IHostApplicationLifetime lifetime)
        {
            _dbContext = dbContext;
            _lifetime = lifetime;
        }

        /// <summary>
        /// Returns health status, database connectivity, and uptime.
        /// </summary>
        /// <returns>Health status JSON.</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Check MongoDB connection status
            var dbStatus = await _dbContext.PingAsync() ? "Connected" : "Disconnected";
            // Calculate uptime since service start
            var uptime = DateTime.UtcNow - _start;
            var payload = new { status = "UP", dbStatus, uptime = $"{(int)uptime.TotalSeconds}s" };
            // Return consistent API response
            return Ok(ApiResponse<object>.Ok(payload));
        }
    }
}
