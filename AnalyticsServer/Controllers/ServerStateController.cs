using AnalyticsServer.Cache;
using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public IActionResult Get()
        {
            
            return Ok(ServerCache.GetAllServers());

            //return Ok(ServerCache.ServerHardwear());



        }
        
        
       
    }
}
