using AnalyticsServer.Cache;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerStateController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(ServerCache.GetAllServers());
        }
    }
}
