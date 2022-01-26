
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDbService : BackgroundService
    {
        private readonly ChannelReader<HWModel> _channelReader;

        public HWDbService(Channel<HWModel> channel)
        {
            _channelReader = channel.Reader;
        }
        
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                     
                    var message =  _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine(message);
                    
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"exception occured :  {ex}"); 
            }
            
           

            
        }
        
    }
}
