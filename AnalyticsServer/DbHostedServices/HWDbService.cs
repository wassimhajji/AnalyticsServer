
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Threading.Channels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace AnalyticsServer.DbHostedServices
{
    public class HWDbService : BackgroundService 
    {
        
        private readonly ChannelReader<HWModel> _channelReader;
       // private ModelBuilder _modelBuilder;
        private MessagesDb _context;
        public Hardware _message;
        IWebHostEnvironment _env;
        IApplicationBuilder _app;
        private object _sender;
        private EventArgs _e;

        public HWDbService(Channel<HWModel> channel, MessagesDb context, Hardware message
            , IWebHostEnvironment env,
            IApplicationBuilder app,
            object sender,
            EventArgs e)
        {
            _channelReader = channel.Reader;
           // _modelBuilder = modelBuilder;
            _context = context;
            _message = message;
            _env = env;
            _app = app;
            _sender = sender;
            _e = e; 
            
        }

        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            

            try
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {

                    var msg = await _channelReader.ReadAsync(stoppingToken);

                    Console.WriteLine($"here is the state : {msg.State}");

                    //await _context.SaveChangesAsync(stoppingToken);


                    string connetionString = null;
                    string sql = null;
                    connetionString = "Server=(localdb)\\mssqllocaldb;Database=StatsDatabase;Trusted_Connection=True;MultipleActiveResultSets=true";

                    sql = $"Insert into Hardware(SlaveId " +
                        $"CpuUser, CpuNice, CpuSys, CpuIoWait, CpuIRQ, " +
                        $"CpuSoft, CpuSteal, CpuGuest, CpuGnice, CpuIdle, " +
                        $"RamTotal, RamUsed, RamCache, RamSwap, RamBoot, " +
                        $"IONetIn, IONetOut, IOTime, IODiskRead, IODiskWrite) " +
                        $"Value({msg.SlaveId},{msg.State.Cpu.User},{msg.State.Cpu.Nice}," +
                        $"{msg.State.Cpu.Sys},{msg.State.Cpu.IoWait},{msg.State.Cpu.IRQ}," +
                        $"{msg.State.Cpu.Soft},{msg.State.Cpu.Steal},{msg.State.Cpu.Guest}," +
                        $"{msg.State.Cpu.Gnice},{msg.State.Cpu.Idle},{msg.State.Ram.Total}," +
                        $"{msg.State.Ram.Used},{msg.State.Ram.Cache},{msg.State.Ram.Swap}," +
                        $"{msg.State.Ram.Boot},{msg.State.Io.NetIn},{msg.State.Io.NetOut}," +
                        $"{msg.State.Io.Time},{msg.State.Io.DiskRead},{msg.State.Io.DiskWrite}";

                    using (SqlConnection cnn = new SqlConnection(connetionString))

                        try
                    {
                            cnn.Open();

                            using(SqlCommand cmd = new SqlCommand(sql, cnn))
                            {
                                cmd.Parameters.Insert(0, msg);  
                                SqlDataAdapter adapter = new SqlDataAdapter();
                                adapter.InsertCommand = new SqlCommand(sql);
                                adapter.InsertCommand.ExecuteNonQuery();
                                cmd.Dispose();

                            }



                            /*if (!string.IsNullOrWhiteSpace(msg.SlaveId))
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
                                _message.IODiskWrite = msg.State.Io.DiskWrite;*/
                            //Console.WriteLine(_message);
                            // await _context.AddAsync(_message);
                            // await _context.SaveChangesAsync(stoppingToken);



                        }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    }

                }                
            }
            catch (Exception ex)
            {

                Console.WriteLine($"exception occured :  {ex}"); 
            }  
        }

        
        //public async Task<>  
        protected  void PageLoad(object sender, EventArgs e,string request)
        {
            SqlConnection cnn;
            SqlCommand  Command;
            SqlDataAdapter adapter = new SqlDataAdapter();
            string sql = "";
            sql = request;
            Command = new SqlCommand(sql);
             adapter.InsertCommand  = new SqlCommand(sql);
            adapter.InsertCommand.ExecuteNonQuery();

            Command.Dispose();
            
            

        }
       
    }
    
}


