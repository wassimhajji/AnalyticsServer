using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.HostedServices
{
    public class HardwareGeneral : BackgroundService
    {
        private readonly ChannelReader<StreamMessages> _channelReader;
        private readonly ChannelReader<HWModel> channelReader;
        private readonly ChannelReader<ConcurrentDictionary<string, UsersConnection>> _usersChannelReader;
        private readonly ConcurrentDictionary<string , SlaveList>? Slaves = new() ;
        public HardwareGeneral(Channel<StreamMessages> channel, Channel<HWModel> _channel, Channel<ConcurrentDictionary<string, UsersConnection>> usersChannelReader)
        {
            _channelReader = channel.Reader;
            channelReader = _channel.Reader;
            _usersChannelReader = usersChannelReader;   
        }

        private void UpdateGeneralHardware(CancellationToken stoppingToken)
        {
            
        _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var HWMsg = await channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the general Hardware cache is{HWMsg}");
                    var StreamMsg = await _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the general stream cache{StreamMsg}");
                    var UsersMsg = await _usersChannelReader.ReadAsync(stoppingToken);  
                    Console.WriteLine($"the message update users is = {UsersMsg}");
                    UsersConnection user = new UsersConnection();
                    int TotalOnlineConnections = 0;
                    int TotalOnlineUsers = 0;
                    foreach (var item in UsersMsg.Keys)
                    {
                        if (item == HWMsg.SlaveId)
                        {
                              user = UsersMsg[item];
                            

                        }
                    }


                    TotalOnlineConnections = TotalOnlineConnections + user.NbConnections;
                    TotalOnlineUsers = TotalOnlineUsers + user.NbUsers;
                    UsersConnections userGeneral = new UsersConnections
                    {
                        OnlineUsers = user.NbUsers,
                        OnlineConnections = user.NbConnections,
                    };

                    int notWorking = 0;
                    int working = 0;
                   /* foreach (var item in StreamMsg.State)
                    {
                        if (item.Time == null) notWorking++;
                        if (item.Time != null) working++;
                    }
                    StreamsWorking streams = new StreamsWorking
                    {
                        Working = working,
                        NotWorking = notWorking,
                    };*/

                    ConcurrentDictionary<string, MessagesModels.StreamMessages> Streams = new();
                    Streams = Cache.StreamCache.GetAllStreams();
                    foreach (var item in Streams.Keys)
                    {

                    }
                    
                    /*for (int i = 0; i < HWMsg.State.Disks.Count; i++)
                    {
                        for (int j = 0; j < HWMsg.State.Disks[i].Size.ToString().Length; j++)
                        {
                            if (char.IsDigit(HWMsg.State.Disks[i].Size.ToString()[j]))
                            {
                                strSize += HWMsg.State.Disks[i].Size.ToString()[j];
                            }
                        }
                        DiskSize = int.Parse(strSize);
                        DiskSizeTotal = DiskSize + DiskSizeTotal;
                        for (int j = 0; j < HWMsg.State.Disks[i].Available.ToString().Length; j++)
                        {
                            if (char.IsDigit(HWMsg.State.Disks[i].Available.ToString()[j]))
                            {
                                strAvailable += HWMsg.State.Disks[i].Available.ToString()[j];
                            }
                        }
                        DiskAvailable = int.Parse(strAvailable);
                        DiskAvailableTotal = DiskAvailable + DiskAvailableTotal;
                    }*/

                    Cache.Models.Cpu cpu = new Cache.Models.Cpu
                    {
                        Gnice = HWMsg.State.Cpu.Gnice,
                        Guest = HWMsg.State.Cpu.Guest,
                        Idle = HWMsg.State.Cpu.Idle,
                        IoWait = HWMsg.State.Cpu.IoWait,
                        IRQ = HWMsg.State.Cpu.IRQ,
                        Nice = HWMsg.State.Cpu.Nice,
                        Soft = HWMsg.State.Cpu.Soft,
                        Steal = HWMsg.State.Cpu.Steal,
                        Sys = HWMsg.State.Cpu.Sys,
                        User = HWMsg.State.Cpu.User,

                    };

                    Cache.Models.Io io = new Cache.Models.Io
                    {
                        DiskRead = HWMsg.State.Io.DiskRead,
                        DiskWrite = HWMsg.State.Io.DiskWrite,
                        NetIn = HWMsg.State.Io.NetIn,
                        NetOut = HWMsg.State.Io.NetOut,
                        Time = HWMsg.State.Io.Time,

                    };
                    Cache.Models.Ram ram = new Cache.Models.Ram
                    {
                        Boot = HWMsg.State.Ram.Boot,
                        Cache = HWMsg.State.Ram.Cache,
                        Swap = HWMsg.State.Ram.Swap,
                        Total = HWMsg.State.Ram.Total,
                        Used = HWMsg.State.Ram.Used,
                    };
                    List<Cache.Models.Disk> listDisk = new List<Cache.Models.Disk>();
                    foreach (var item in HWMsg.State.Disks)
                    {
                        Cache.Models.Disk disk = new Cache.Models.Disk
                        {
                            FileSystem = item.FileSystem,
                            Size = item.Size,
                            Used = item.Used,
                            Available = item.Available,
                            Use = item.Use,
                            MontedOn = item.MontedOn,

                        };
                        listDisk.Add(disk);
                    }

                    SlaveState state = new SlaveState
                    {
                        Cpu = cpu,
                        Io = io,
                        Ram = ram,
                        Disk = listDisk,
                        
                    };
                    UsersConnections UserUpdated = new UsersConnections();
                    foreach (var item in UsersMsg.Keys)
                    {
                        UserUpdated.OnlineUsers = UsersMsg[item].NbUsers;
                        UserUpdated.OnlineConnections = UsersMsg[item].NbConnections;   
                    }
                    SlaveList slavelist = new SlaveList
                    {
                        SlaveId = HWMsg.SlaveId,
                        State = state,
                        Streams =null,
                        UsersConnections = UserUpdated,
                    };
                    foreach (var item in Streams.Keys)
                    {
                        if (item == slavelist.SlaveId)
                        {
                            foreach (var itm in Streams[item].State)
                            {
                                if (itm.Time != null) working ++;
                                else notWorking ++;
                                
                            }
                        }
                    }
                    StreamsWorking streams = new StreamsWorking
                    {
                        Working = working,
                        NotWorking = notWorking,
                    };


                    var newSlaves = new SlaveList {    SlaveId = HWMsg.SlaveId, State = state, Streams = streams, UsersConnections = userGeneral };

                    if (Slaves.TryGetValue( HWMsg.SlaveId, out var slaveList))
                    {
                        Slaves.TryUpdate(HWMsg.SlaveId, newSlaves, slaveList);
                        return;

                    }

                    Slaves.TryAdd(HWMsg.SlaveId, newSlaves);

                    int netInTotal = 0;
                    int netOuTotal = 0;
                    int DiskSize = 0;
                    //int DiskAvailable = 0;
                    int DiskSizeTotal = 0;
                    int DiskAvailableTotal = 0;

                    string strSize = string.Empty;
                    string strAvailable = string.Empty;
                    foreach (var item in Slaves.Keys)
                    {
                        for (int i = 0; i < Slaves[item].State.Disk.Count; i++)
                        {
                            for (int j = 0; j < Slaves[item].State.Disk[i].Size.ToString().Length; j++)
                            {
                                if (char.IsDigit(HWMsg.State.Disks[i].Size.ToString()[j]) )
                                {
                                    strSize += HWMsg.State.Disks[i].Size.ToString()[j];
                                    
                                }
                                
                            }
                            DiskSize = int.Parse(strSize);
                            DiskSize = int.Parse(strSize);
                            
                            DiskSizeTotal = DiskSize + DiskSizeTotal;
                        }
                    }





                    
                    var  usr = 0;
                    var conn = 0;
                    foreach (var item in UsersMsg.Keys)
                    {
                        usr = usr + UsersMsg[item].NbUsers;
                        conn = conn + UsersMsg[item].NbConnections;
                    }
                    
                    
                    
                    Cache.Models.Index index = new Cache.Models.Index
                    {
                        NetInTotal = netInTotal + HWMsg.State.Io.NetIn,
                        NetOutTotal = netOuTotal + HWMsg.State.Io.NetOut,
                        DiskCapacityTotal = DiskSizeTotal,
                        AvailableTotal = DiskAvailableTotal,
                        TotalOnlineUsers = usr,
                        TotalOnlineConnections = conn,
                        Slaves = Slaves,
                        

                    };
                    Cache.HardwareCache.UpdateServerHardwear(Slaves);   
                }
            });
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            UpdateGeneralHardware(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
