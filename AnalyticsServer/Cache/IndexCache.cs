using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class IndexCache
    {
        // private static Cache.Models.Index index = new Cache.Models.Index(); 
        private static ConcurrentDictionary<string, SlaveList> Slaves = new();
        public static   Cache.Models.Index updateIndex()
        {
            int Intotal = 0;
            int outTotal = 0;
            int size = 0;
            int dispo = 0;
            int users = 0;
            int connections = 0;

            var hard = Cache.ServerCache.GetAllServers();
            var User = Cache.UsersConnectionCache.GetAllUsersAndConnections();

            

            foreach (var item in hard)
            {
                Intotal = Intotal + item.Value.Hardwear.State.Io.NetIn;
                outTotal = outTotal + item.Value.Hardwear.State.Io.NetOut;
            }
            
            var user = Cache.UsersConnectionCache.GetAllUsersAndConnections();
            foreach (var item in user)
            {
                users = users + item.Value.NbUsers;
                connections = connections + item.Value.NbConnections;   
            }

            

            //SlaveList slaves = new SlaveList();
            List<SlaveList> list = new List<SlaveList>();

            foreach (var item in hard)
            {
                UsersConnections usr = new UsersConnections();
                foreach (var itm in User.Keys)
                {
                    if (itm == item.Value.SlaveId)
                    {
                        users = User[itm].NbUsers; 
                        connections = User[itm].NbConnections;
                    }
                    usr.OnlineUsers = users;
                }
               
               /* SlaveList slaves = new SlaveList()
                {
                    SlaveId = item.Key,
                    State = item.Value.Hardwear,
                };
                var newState = new SlaveList { SlaveId = slaves.SlaveId, State = slaves.State , UsersInfo = usr };
                if (Slaves.TryGetValue(slaves.SlaveId, out var state))
                {
                    Slaves.TryUpdate(slaves.SlaveId, newState, state);
                    

                }*/

               // Slaves.TryAdd(slaves.SlaveId, newState);
            }


            
            


            /*
            List<SlaveList> list = new List<SlaveList>();
            List<Cache.Models.Disk> diskList = new List<Cache.Models.Disk>();
            Cache.Models.Disk disk = new Cache.Models.Disk();   
            SlaveState states = new SlaveState();
            foreach (var item in hard)
            {
                states.Cpu.Steal = item.Value.Hardwear.State.Cpu.Steal;
                states.Cpu.IRQ = item.Value.Hardwear.State.Cpu.IRQ;
                states.Cpu.IoWait = item.Value.Hardwear.State.Cpu.IoWait;
                states.Cpu.Idle = item.Value.Hardwear.State.Cpu.Idle;
                states.Cpu.Guest = item.Value.Hardwear.State.Cpu.Guest;
                states.Cpu.Nice = item.Value.Hardwear.State.Cpu.Nice;
                states.Cpu.User = item.Value.Hardwear.State.Cpu.User;
                states.Cpu.Gnice = item.Value.Hardwear.State.Cpu.Gnice;
                states.Cpu.Soft = item.Value.Hardwear.State.Cpu.Soft;
                states.Cpu.Sys = item.Value.Hardwear.State.Cpu.Sys;

                states.Io.NetIn = item.Value.Hardwear.State.Io.NetIn;
                states.Io.NetOut = item.Value.Hardwear.State.Io.NetOut;
                states.Io.Time = item.Value.Hardwear.State.Io.Time;
                states.Io.DiskWrite = item.Value.Hardwear.State.Io.DiskWrite;
                states.Io.DiskRead = item.Value.Hardwear.State.Io.DiskRead;

                states.Ram.Swap = item.Value.Hardwear.State.Ram.Swap;
                states.Ram.Total = item.Value.Hardwear.State.Ram.Total;
                states.Ram.Used = item.Value.Hardwear.State.Ram.Used;
                states.Ram.Cache = item.Value.Hardwear.State.Ram.Cache;
                states.Ram.Boot = item.Value.Hardwear.State.Ram.Boot;

                foreach (var itm in item.Value.Hardwear.State.Disks)
                {
                    disk.Used = itm.Used;
                    disk.MontedOn = itm.MontedOn;   
                    disk.Size = itm.Size;   
                    disk.FileSystem = itm.FileSystem;   
                    disk.Available = itm.Available; 
                    disk.Use = itm.Use; 
                    diskList.Add(disk); 
                }
                states.Disk = diskList;

            }*/

            

            Cache.Models.Index index = new Models.Index
            {
                TotalOnlineUsers = users,
                TotalOnlineConnections = connections,
                NetInTotal = Intotal,
                NetOutTotal = outTotal,
                Slaves = Slaves,
            };
            return index;
        }
        /*public static Cache.Models.Index GetIndex()
        {
            return index;
        }*/
    }
}
