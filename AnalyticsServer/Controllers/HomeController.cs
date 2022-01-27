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
       // private ModelBuilder _modelBuilder;

        public HomeController(Channel<HWModel> channel)
        {
            _channelReader = channel.Reader;
            
        }
        public async Task<IActionResult> Index(CancellationToken stoppingToken)
        {
            try
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {

                    var message = _channelReader.ReadAsync(stoppingToken);
                    //return Ok(message);
                    //return Ok(message.Result);
                    var msg = message.Result;
                    Console.WriteLine(msg.SlaveId);
                    return Ok(msg.SlaveId);




                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"exception occured :  {ex}");
            }



            return Ok(true);

            
        }
    }
}
