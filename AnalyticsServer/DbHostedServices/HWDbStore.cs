using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDbStore : BackgroundService
    {
       
        private async Task ListenForConsumer(CancellationToken stoppingToken,[FromServices] Channel<HWModel> _channel)
        {
            while (!_channel.Reader.Completion.IsCompleted)
            {

                try
                {
                     var message = await _channel.Reader.ReadAsync();


                    Console.WriteLine(message);
                }
                catch (Exception ex)
                {

                    Console.WriteLine(ex);
                }
                
            }
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
