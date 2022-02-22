using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace AnalyticsServer.Cache
{
    public class ServerCache
    {
        

        private static ConcurrentDictionary<string, ServerState> Servers = new();
        private static ConcurrentDictionary<string, DiskCpuTotal> disk = new();
        private static DiskCpuTotal diskCpu = new DiskCpuTotal();
        public static void UpdateServerHardwear(HWModel model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            if (model.State == null) return;
            var newState = new ServerState { SlaveId = model.SlaveId, Hardwear = model   };

            if (Servers.TryGetValue(model.SlaveId, out var state))
            {
                Servers.TryUpdate(model.SlaveId, newState, state);
                return;

            }

            Servers.TryAdd(model.SlaveId, newState);

            int netInTotal = 0;
            int netOutTotal = 0;

            foreach (var item in Servers.Values)
            {
                netInTotal = netInTotal + item.Hardwear.State.Io.NetIn;
                netOutTotal = netOutTotal + item.Hardwear.State.Io.NetOut;
            } 

            var newDisk = new DiskCpuTotal { NetOutTotal = netOutTotal, NetInTotal = netInTotal , AvailableTotal = 0, DiskCapacityTotal = 0 };
            if (disk.TryGetValue(model.SlaveId, out var disks))
            {
                disk.TryUpdate(model.SlaveId, newDisk, disks);
                return;

            }

            disk.TryAdd(model.SlaveId, newDisk);

        }

        public static ConcurrentDictionary<string, ServerState> GetAllServers()
        {
            return Servers;
        }

       public static ConcurrentDictionary<string, DiskCpuTotal> GetDiskCpuTotal()
        {
            return disk;

        }
        

    }
}
