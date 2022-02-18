using AnalyticsServer.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamsController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return Ok(StreamCache.GetAllStreams());
        }
    }
}
