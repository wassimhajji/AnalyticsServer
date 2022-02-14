using AnalyticsServer.Cache.Models;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class HardwareCache
    {
        private static ConcurrentDictionary<string, Cache.Models.Index> General = new();
        public static void UpdateServerHardwear(Cache.Models.Index model)
        {
            if (model == null) return;
            
            if (model.Slaves == null) return;
            var newState = new Cache.Models.Index { NetInTotal = model.NetInTotal , NetOutTotal = model.NetOutTotal ,DiskCapacityTotal=model.DiskCapacityTotal, AvailableTotal=model.AvailableTotal
            ,Slaves= model.Slaves};

            if (General.TryGetValue(model.NetOutTotal.ToString(), out var state))
            {
                General.TryUpdate("General state", newState, state);
                return;

            }

            General.TryAdd("General state", newState);
        }
        public static ConcurrentDictionary<string, Cache.Models.Index> GetAllHardwares()
        {
            return General;
        }
    }
}
