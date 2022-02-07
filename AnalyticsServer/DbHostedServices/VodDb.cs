using AnalyticsServer.MessagesModels;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class VodDb : BackgroundService
    {
        private readonly ChannelReader<VodMessage> _channelReader;

        public VodDb(Channel<VodMessage> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            //_db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the vod message is : {msg.State.DownloadList}");
                }
            });
            
           


          
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ReadAndSaveMessages(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
