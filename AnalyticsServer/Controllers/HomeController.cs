using AnalyticsServer.MessagesDatabase;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace AnalyticsServer.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : Controller
    {
        private readonly ChannelReader<HWModel> _channelReader;

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
                    return Ok(message);
                    //Console.WriteLine(message);

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
