using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamGroupingController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(Cache.StreamGroupingCache.GetAllStreamGroupings());
        }
    }
}
