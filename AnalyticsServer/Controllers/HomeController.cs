using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Channels;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ChannelReader<HWModel> _channelReader;
        private ModelBuilder _modelBuilder; 
        private MessagesDb _context; 

        

        public HomeController(Channel<HWModel> channel, MessagesDb context)
        {
            _channelReader = channel.Reader;
            
            _context = context; 
        }
        public async Task<IActionResult> Index(CancellationToken stoppingToken)
        {


            var msg = await _channelReader.ReadAsync(stoppingToken);
            

            Console.WriteLine($"here is the state : {msg.State.Ram}");
            _context.SaveChanges();
            return Ok(msg);
        }
    }
}
