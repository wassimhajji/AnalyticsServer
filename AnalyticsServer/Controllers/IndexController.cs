using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IndexController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(Cache.HardwareCache.GetAllHardwares());

        }
    }
}
