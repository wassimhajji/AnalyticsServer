using AnalyticsServer.Cache;
using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

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
