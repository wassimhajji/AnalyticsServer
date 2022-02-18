using AnalyticsServer.Cache;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamStateController : Controller
    {
       private static ConcurrentDictionary<string, MessagesModels.StreamMessages> Streams = new();


        [HttpGet]
        [Authorize]
        public IActionResult Get(int Id)
        {
           return Ok(StreamCache.GetStreams(Id));

           
        }
    }
}
