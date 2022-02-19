using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class UsersConnectionDb : BackgroundService
    {
        private readonly ChannelReader<ConcurrentDictionary<string, UsersConnection>> _channelReader;
        private readonly MessagesDb _db;
        public UsersConnectionDb(Channel<ConcurrentDictionary<string, UsersConnection>> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;
            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private async void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ = Task.Run(async() =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    if (msg == null) return;
                   /* foreach (var item in msg.Keys)
                    {
                        UsersConnectionModel user = new UsersConnectionModel
                        {
                            Id = Guid.NewGuid(),
                            SlaveId = msg.Keys.FirstOrDefault(),
                            UsersNumber = msg.Values
                            ConnectionsNumber = msg.Values.FirstOrDefault().NbConnections,

                        };
                    } */

                    string str = msg.Keys.First();
                    var model = msg[str];
                    
                    UsersConnectionModel user = new UsersConnectionModel
                    {
                        Id = Guid.NewGuid(),
                        SlaveId = str, 
                        UsersNumber = model.NbUsers,
                        ConnectionsNumber = model.NbConnections,
                        UnusedSessions = model.UnusedSessions,
                        
                        TimeAdded = DateTime.Now,
                    };
                    await _db.UsersConnection.AddAsync(user);
                    try
                    {
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    };
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
