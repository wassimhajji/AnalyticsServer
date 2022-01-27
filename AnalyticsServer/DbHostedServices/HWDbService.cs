
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDbService : BackgroundService 
    {
        private readonly ChannelReader<HWModel> _channelReader;
        private   ModelBuilder _modelBuilder;

        public HWDbService(Channel<HWModel> channel,ModelBuilder modelBuilder)
        {
            _channelReader = channel.Reader;
            _modelBuilder = modelBuilder;
        }

        private static ConcurrentDictionary<string, HWModel> NewMessage = new();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            
            try
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                     
                    var message =  _channelReader.ReadAsync(stoppingToken);
                   var  msg = message.Result;


                    if (string.IsNullOrWhiteSpace(msg.SlaveId))
                    {
                        _modelBuilder.Entity<Hardware>().HasData(
                        new Hardware
                        {

                            SlaveId = msg.SlaveId,
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


                        });
                    }

                    
                    
                    
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"exception occured :  {ex}"); 
            }
            
           

            
        }


        
    }
}
