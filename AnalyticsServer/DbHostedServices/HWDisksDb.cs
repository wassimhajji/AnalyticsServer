using AnalyticsServer.MessagesDatabase;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDisksDb : BackgroundService
    {
        private readonly ChannelReader<HWModel> _channelReader;

        private MessagesDb _db;


        public HWDisksDb(Channel<HWModel> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }
        private void ReadAndSaveMessages(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    foreach (var item in msg.State.Disks)
                    {
                        var model = _db.HardwareDisks.OrderByDescending(s => s.TimeAdded).FirstOrDefault();
                        Console.WriteLine($"the last row is {model}");

                        HardwareDisks disk = new HardwareDisks
                        {
                            Id = Guid.NewGuid(),
                            SlaveId = msg.SlaveId,
                            FileSystem = item.FileSystem,
                            Size = item.Size,
                            Used = item.Used,
                            Available = item.Available,
                            Use = item.Use,
                            MontedOn = item.MontedOn,
                            TimeAdded = DateTime.Now,
                        };
                        if (model.SlaveId == disk.SlaveId && model.Used == disk.Used && model.Available == disk.Available && model.MontedOn == disk.MontedOn && model.FileSystem == disk.FileSystem
                             && model.Use == disk.Use) return;

                        await _db.HardwareDisks.AddAsync(disk);
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
        protected async  override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ReadAndSaveMessages(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
