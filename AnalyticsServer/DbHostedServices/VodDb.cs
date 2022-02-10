using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Newtonsoft.Json;
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
                        Vod vod = new Vod
                        {
                            VodId = Guid.NewGuid(),
                            SlaveId = msg.SlaveId,
                            ExistantList = str,
                        };
                        await _db.Vod.AddAsync(vod);
                        Console.WriteLine(vod);
                        try
                        {
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex);
                        }
                        //await _db.Vods.AddAsync(vod);
                        //Existant existant = new Existant();
                        //Console.WriteLine($"number of elements is = {msg.State.Existing.Count}");
                        //string str = JsonConvert.SerializeObject(msg.State.Existing);
                        Console.WriteLine($"the existant list is : {str}");

                        /*foreach (var item in msg.State.Existing)
                        {
                            existant.ExistantListId = vod.ExistantListId;
                            existant.SlaveId = vod.SlaveId;
                            existant.ExistantId = item;
                            await _db.ExistantList.AddRangeAsync(existant);
                        }*/
                        //Console.WriteLine(existant);    
                        //await _db.Vods.AddAsync(vod);
                        //await _db.ExistantList.AddAsync(existant);
                       /* try
                        {
                            await _db.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex);
                        }*/
                    });
                    /*try
                    {
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    }*/
                }
                
            });

            /*try
            {
               await _db.SaveChangesAsync();    
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }*/


          
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
