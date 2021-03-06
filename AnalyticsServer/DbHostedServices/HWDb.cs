using AnalyticsServer.MessagesDatabase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDb : BackgroundService
    {
        private readonly ChannelReader<HWModel> _channelReader;
        
        private MessagesDb _db;
        

        public HWDb(Channel<HWModel> channel, IServiceProvider serviceProvider)
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

                    Hardware hardware = new Hardware
                    {
                        Id = Guid.NewGuid(),
                        SlaveId = msg.SlaveId,
                        TimeAdded = DateTime.Now,
                        CpuUser = msg.State.Cpu.User,
                        CpuNice = msg.State.Cpu.Nice,
                        CpuSys = msg.State.Cpu.Sys,
                        CpuIoWait = msg.State.Cpu.IoWait,
                        CpuIRQ = msg.State.Cpu.IRQ,
                        CpuSoft = msg.State.Cpu.Soft,
                        CpuSteal = msg.State.Cpu.Steal,
                        CpuGuest = msg.State.Cpu.Guest,
                        CpuGnice = msg.State.Cpu.Gnice,
                        CpuIdle = msg.State.Cpu.Idle,
                        RamTotal = msg.State.Ram.Total,
                        RamUsed = msg.State.Ram.Used,
                        RamCache = msg.State.Ram.Cache,
                        RamSwap = msg.State.Ram.Swap,
                        RamBoot = msg.State.Ram.Boot,
                        IONetIn = msg.State.Io.NetIn,
                        IONetOut = msg.State.Io.NetOut,
                        IOTime = msg.State.Io.Time,
                        IODiskRead = msg.State.Io.DiskRead,
                        IODiskWrite = msg.State.Io.DiskWrite,
                    };
                    Console.WriteLine($"hardware from db is : {hardware}");

                    await _db.Hardware.AddAsync(hardware);
                    try
                    {
                        await _db.SaveChangesAsync();
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
