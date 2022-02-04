using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerStateHistoryController : Controller
    {
        
        private readonly MessagesDb _db;
        public ServerStateHistoryController(MessagesDb db)
        {
            _db = db;
        }
        public IActionResult Index(string Id, int minutes)
        {
            var RollBack = DateTime.Now.AddMinutes(-minutes);
            return Ok((
            from p in _db.Hardware
            where (p.SlaveId == Id) && (DateTime.Compare(p.TimeAdded, RollBack) > 0)
            select p
        ).ToList());
        }
    }
}
