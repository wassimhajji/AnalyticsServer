using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Mvc;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HardwareDisksStateHistoryController : Controller
    {
        private readonly MessagesDb _db;
        public HardwareDisksStateHistoryController(MessagesDb db)
        {
            _db = db;
        }
        
        public IActionResult Index(string Id, int minutes)
        {
            var RollBack = DateTime.Now.AddMinutes(-minutes);
            return Ok((
            from p in _db.HardwareDisks
            where (p.SlaveId == Id) && (DateTime.Compare(p.TimeAdded, RollBack) > 0)
            select p
        ).ToList());
        }
    }
}
