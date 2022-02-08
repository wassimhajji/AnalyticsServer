using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ChannelReader<VodMessage> _channelReader;
        private ModelBuilder _modelBuilder; 
        private MessagesDb _context; 

        

        public HomeController(Channel<VodMessage> channel, MessagesDb context)
        {
            _channelReader = channel.Reader;
            
            _context = context; 
        }
        public async Task<IActionResult> Index(CancellationToken stoppingToken)
        {
            var msg = await _channelReader.ReadAsync(stoppingToken);
            
            Console.WriteLine(msg);
            return Ok(msg);
           // Console.WriteLine($"here is the state : {msg.State.Ram}");
            //_context.SaveChanges();
            //return Ok(Cache.GeneralCache.GetGeneral());
        }
    }
}
