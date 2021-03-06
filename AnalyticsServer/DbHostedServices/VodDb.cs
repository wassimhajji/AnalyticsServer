using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class VodDb : BackgroundService
    {
        private readonly ChannelReader<VodMessage> _channelReader;
        private readonly MessagesDb _db;

        public VodDb(Channel<VodMessage> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private async void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
               
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                                   
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the vod message is : {msg.State.DownloadList}");
                    _ = Task.Run(async () =>
                    {
                        string str = JsonConvert.SerializeObject(msg.State.Existing);

                        var model = _db.Vod.OrderByDescending(s => s.TimeAdded).FirstOrDefault();
                        /*
                        Select(b => new
                        {
                            Id = b.VodId,
                            SlaveId = b.SlaveId,
                            ExistantList = b.ExistantList,
                            
                        }).FirstOrDefault();
                        */
                        Console.WriteLine(model);

                        


                       // if (model.ExistantList == str && msg.SlaveId == model.SlaveId) return;
                        if (model == null || model.ExistantList != str || msg.SlaveId != model.SlaveId)
                        {
                            Vod vod = new Vod
                            {
                                VodId = Guid.NewGuid(),
                                SlaveId = msg.SlaveId,
                                ExistantList = str,
                                TimeAdded = DateTime.Now,
                            };
                            await _db.Vod.AddAsync(vod);
                            try
                            {
                                await _db.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }
                        }
                        else if (model.ExistantList == str && msg.SlaveId == model.SlaveId) return;
                        Console.WriteLine($"the existant list is : {str}");
                    });
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
