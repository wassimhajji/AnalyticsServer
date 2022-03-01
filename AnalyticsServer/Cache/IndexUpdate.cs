using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class IndexUpdate
    {
        private static ConcurrentDictionary<string, SlaveList> ServersList = new();

        public static void UpdateIndex(HWModel model)
        {

            var streams = Cache.StreamCache.GetAllStreams();
            StreamsWorking streamsWorking = new StreamsWorking();
            foreach (var stream in streams)
            {
                if (model.SlaveId == stream.Key)
                {
                    foreach (var item in stream.Value.State)
                    {
                        if (item.Time != null) streamsWorking.Working++;
                        else streamsWorking.NotWorking++;
                    }
                }
            }




            var users = Cache.UsersConnectionCache.GetAllUsersAndConnections();
            UsersConnections userPerSlave = new UsersConnections();
            foreach (var user in users)
            {
                if (model.SlaveId == user.Key)
                {
                    userPerSlave.OnlineUsers = user.Value.NbUsers;
                    userPerSlave.OnlineConnections = user.Value.NbConnections;
                };
            }

            Cache.Models.HardwareModels.cpu Cpu = new cpu()
            {
                User = model.State.Cpu.User,
                Nice = model.State.Cpu.Nice,
                Sys = model.State.Cpu.Sys,
                IoWait = model.State.Cpu.IoWait,
                IRQ = model.State.Cpu.IRQ,
                Soft = model.State.Cpu.Soft,
                Steal = model.State.Cpu.Steal,
                Guest = model.State.Cpu.Guest,
                Gnice = model.State.Cpu.Gnice,
                Idle = model.State.Cpu.Idle,
            };

            Cache.Models.HardwareModels.Ram Ram = new Models.HardwareModels.Ram()
            {
                Total = model.State.Ram.Total,
                Used = model.State.Ram.Used,
                Cache = model.State.Ram.Cache,
                Swap = model.State.Ram.Swap,
                Boot = model.State.Ram.Boot,
            };

            Cache.Models.HardwareModels.Io Io = new Models.HardwareModels.Io()
            {
                NetIn = model.State.Io.NetIn,
                NetOut = model.State.Io.NetOut,
                Time = model.State.Io.Time,
                DiskRead = model.State.Io.DiskRead,
                DiskWrite = model.State.Io.DiskWrite,
            };

            List<Cache.Models.HardwareModels.Disk> Disks = new List<Models.HardwareModels.Disk>();

            foreach (var item in model.State.Disks)
            {
                Cache.Models.HardwareModels.Disk disk = new Models.HardwareModels.Disk()
                {
                    FileSystem = item.FileSystem,
                    Size = item.Size,
                    Used = item.Used,
                    Available = item.Available,
                    Use = item.Use,
                    MontedOn = item.MontedOn,
                };
                Disks.Add(disk);    
            }







            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            var newState = new SlaveList { SlaveId = model.SlaveId, Cpu = Cpu, Ram = Ram , Io = Io , Disk = Disks, UsersInfo = userPerSlave, Streams = streamsWorking };



            if (ServersList.TryGetValue(model.SlaveId, out var state))
            {
                ServersList.TryUpdate(model.SlaveId, newState, state);
                return;
            }
            ServersList.TryAdd(model.SlaveId, newState);



        }

        public static ConcurrentDictionary<string, SlaveList> GetAllSlaves()
        {
            return ServersList;
        }

        public static Cache.Models.Index GetIndex()
        {
            Queue<decimal> qSize = new System.Collections.Generic.Queue<decimal>();
            Queue<decimal> qAvailable = new System.Collections.Generic.Queue<decimal>();
            
            var index = new Cache.Models.Index();
            foreach (var slave in ServersList)
            {
                foreach (var disk in slave.Value.Disk)
                {
                    if (disk.Size.Contains("G"))
                    {
                        var str = disk.Size.Remove(disk.Size.Length - 1, 1);
                        var num = decimal.Parse(str);
                        qSize.Enqueue(num);
                    }

                    if (disk.Size.Contains("M"))
                    {
                        var str = disk.Size.Remove(disk.Size.Length - 1, 1);
                        var num = decimal.Parse(str)/1024;
                        qSize.Enqueue(num);
                    }
                    if (disk.Available.Contains("G"))
                    {
                        var strr = disk.Available.Remove(disk.Available.Length - 1, 1);
                        var numm = decimal.Parse(strr);
                        qAvailable.Enqueue(numm);
                    }
                    if (disk.Available.Contains("M"))
                    {
                        var strr = disk.Available.Remove(disk.Available.Length - 1, 1);
                        var numm = decimal.Parse(strr)/1024;
                        qAvailable.Enqueue(numm);
                    }
                }
                


                
                decimal summ = 0;
                foreach (var num in qSize)
                {

                    summ += num;
                }
                decimal sumAv = 0;
                foreach (var numm in qAvailable)
                {
                    sumAv += numm;
                }
                
                index.DiskCapacityTotal = summ.ToString();
                index.AvailableTotal = sumAv.ToString();
                index.NetInTotal += slave.Value.Io.NetIn;
                index.NetOutTotal += slave.Value.Io.NetOut;
                index.TotalOnlineUsers += slave.Value.UsersInfo.OnlineUsers;
                index.TotalOnlineConnections += slave.Value.UsersInfo.OnlineConnections;





            }
            //index.Slaves = ServersList;
            List<SlaveList> slaves = new List<SlaveList>();
            foreach (var item in ServersList.Values)
            {
                slaves.Add(item);
            }
            index.Slaves = slaves;  
            return index;
        }

            public static string getQueue()
            {
                Queue<decimal> qSize = new System.Collections.Generic.Queue<decimal>();
                foreach (var item in ServersList)
                {
                    foreach (var disk in item.Value.Disk)
                    {

                        var str = disk.Size.Remove(disk.Size.Length - 1, 1);
                        var numm = decimal.Parse(str);
                        qSize.Enqueue(numm);
                    }
                }
                decimal sum = 0;
                string subresult = string.Empty;
                foreach (var item in qSize)
                {
                    sum += item;
                    subresult += $" || the number to sum is {item} ||";
                }

                string result = subresult + $" || la somme est {sum}";

                return result;
            }
        }
    }
