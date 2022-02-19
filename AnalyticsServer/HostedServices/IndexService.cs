using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.HostedServices
{
    public class IndexService : BackgroundService
    {
        private readonly ChannelReader<StreamMessages> _channelReader;
        private readonly ChannelReader<HWModel> channelReader;
        private readonly ChannelReader<ConcurrentDictionary<string, UsersConnection>> _usersChannelReader;

        public IndexService(Channel<StreamMessages> channel, Channel<HWModel> _channel, Channel<ConcurrentDictionary<string, UsersConnection>> usersChannelReader)
        {
            _channelReader = channel.Reader;
            channelReader = _channel.Reader;
            _usersChannelReader = usersChannelReader;
        }
        private void UpdateIndex(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
           {
               while (await _channelReader.WaitToReadAsync(stoppingToken))
               {
                   var HWMsg = await channelReader.ReadAsync(stoppingToken);
                   Console.WriteLine($"the general Hardware cache is{HWMsg}");
               }
           });
        }
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new NotImplementedException();
        }
    }
}
