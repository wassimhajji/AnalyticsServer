using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamGroupingController : Controller
    {
        [HttpGet]
        [Authorize]
        public IActionResult Index()
        {
            return Ok(Cache.StreamGroupingCache.GetAllStreamGroupings());
        }
    }
}
