using AnalyticsServer.Cache;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamsController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(StreamCache.GetAllStreams());
        }
    }
}
