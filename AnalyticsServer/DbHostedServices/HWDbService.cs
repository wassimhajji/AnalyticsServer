
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
       // private ModelBuilder _modelBuilder;
        private MessagesDb _context;
        public Hardware _message;
        public HWDbService(Channel<HWModel> channel, MessagesDb context, Hardware message)
        {
            _channelReader = channel.Reader;
           // _modelBuilder = modelBuilder;
            _context = context;
            _message = message; 
        }

        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            try
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                     
                    var msg = await  _channelReader.ReadAsync(stoppingToken);
                   
                    Console.WriteLine($"here is the state : {msg.State}");

                    //await _context.SaveChangesAsync(stoppingToken);

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(msg.SlaveId))
                        {
                            _message.SlaveId = msg.SlaveId;
                            _message.CpuUser = msg.State.Cpu.User;
                            _message.CpuNice = msg.State.Cpu.Nice;
                            _message.CpuSys = msg.State.Cpu.Sys;
                            _message.CpuIoWait = msg.State.Cpu.IoWait;
                            _message.CpuIRQ = msg.State.Cpu.IRQ;
                            _message.CpuSoft = msg.State.Cpu.Soft;
                            _message.CpuSteal = msg.State.Cpu.Steal;
                            _message.CpuGuest = msg.State.Cpu.Guest;
                            _message.CpuGnice = msg.State.Cpu.Gnice;
                            _message.CpuIdle = msg.State.Cpu.Idle;
                            _message.RamTotal = msg.State.Ram.Total;
                            _message.RamUsed = msg.State.Ram.Used;
                            _message.RamCache = msg.State.Ram.Cache;
                            _message.RamSwap = msg.State.Ram.Swap;
                            _message.RamBoot = msg.State.Ram.Boot;
                            _message.IONetIn = msg.State.Io.NetIn;
                            _message.IONetOut = msg.State.Io.NetOut;
                            _message.IOTime = msg.State.Io.Time;
                            _message.IODiskRead = msg.State.Io.DiskRead;
                            _message.IODiskWrite = msg.State.Io.DiskWrite;



                            //Console.WriteLine(_message);

                            // await _context.AddAsync(_message);
                            // await _context.SaveChangesAsync(stoppingToken);
                        }
                        }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    }
                    




                        /* _ = Task.Run( () =>
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
                         }); 
                         _context.SaveChanges(); 
                         }*/
                    }
                    
                    

                    
                    
                    
                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"exception occured :  {ex}"); 
            }
            
           

            
        }
        //public async Task<>


        
    }
}
