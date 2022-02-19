using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class CountryGroupingDb : BackgroundService
    {
        private readonly ChannelReader<CountryGrouping> _channelReader;
        private readonly MessagesDb _db;

        public CountryGroupingDb(Channel<CountryGrouping> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
           {
               while ((await _channelReader.WaitToReadAsync(stoppingToken)))
               {
                   var msg = await _channelReader.ReadAsync(stoppingToken);
                   Console.WriteLine($"grouping by country  from db is : {msg}");

                   string str = string.Empty;
                   foreach (var item in msg.CountryGroups.Keys)
                   {
                       str += item + " = " + msg.CountryGroups[item] + " | ";
                   }

                   CountryGroupingModel model = new CountryGroupingModel
                   {
                       Id = Guid.NewGuid(),
                       Groups = str,
                       TimeAdded = DateTime.UtcNow, 


                   };
                   //Cache.GroupingByCountryCache.UpdateGroupingByCountry1(model);
                   await _db.CountryGrouping.AddAsync(model);
                   try
                   {
                       _db.SaveChangesAsync();
                   }
                   catch (Exception ex)
                   {
                       Console.WriteLine(ex);
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
