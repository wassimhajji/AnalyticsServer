using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryGroupingController : Controller
    {
        [HttpGet]
        //[Authorize]
        public IActionResult Index()
        {
            return Ok(Cache.GroupingByCountryCache.GetAllCountryGroupings());
            //return Ok(Cache.GroupingByCountryCache.GetAllCountryGroupings1());
            
        }
    }
}
