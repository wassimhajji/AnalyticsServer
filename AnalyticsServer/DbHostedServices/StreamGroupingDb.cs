using AnalyticsServer.MessagesDatabase;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class StreamGroupingDb : BackgroundService
    {
        private readonly ChannelReader<ConcurrentDictionary<string, int>> _channelReader;
        private readonly MessagesDb _db;

        public StreamGroupingDb(Channel<ConcurrentDictionary<string, int>> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private async void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ =  Task.Run(async () =>
            {
                while(await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the stream grouping message from db is {msg}" );
                    foreach (var item in msg.Keys)
                    {
                        StreamGrouping row = new StreamGrouping
                        {
                            Id = Guid.NewGuid(),
                            TimeAdded = DateTime.UtcNow,
                            StreamId = item,
                            UsersNumber = msg[item],
                        };
                        await _db.StreamsGrouping.AddAsync(row);
                        try
                        {
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                    }
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
