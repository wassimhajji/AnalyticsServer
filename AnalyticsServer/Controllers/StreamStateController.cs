using AnalyticsServer.Cache;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamStateController : Controller
    {
       [HttpGet]
       public IActionResult Get()
        {
            return Ok(StreamCache.GetAllStreams());
        }
    }
}
