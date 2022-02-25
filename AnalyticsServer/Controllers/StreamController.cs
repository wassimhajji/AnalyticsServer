using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StreamController : Controller
    {
        private readonly MessagesDb _db;
        public StreamController(MessagesDb db)
        {
            _db = db;
        }
        [HttpGet]
        [Authorize]
        public IActionResult Index(int Id)
        {
            
            return Ok((
            from p in _db.Streams
            where (p.StreamId == Id) 
            select p
        ).ToList());
        }
    }
}
