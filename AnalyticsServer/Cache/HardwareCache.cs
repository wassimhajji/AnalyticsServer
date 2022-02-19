using AnalyticsServer.Cache.Models;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class HardwareCache
    {
        private static ConcurrentDictionary<string, Cache.Models.Index> General = new();
        public static void UpdateServerHardwear(ConcurrentDictionary<string, SlaveList> Slaves )
        {
            int Intotal = 0;
            int outTotal = 0;
            int size = 0;
            int dispo = 0;
            int users = 0;
            int connections = 0;

            foreach (var item in Slaves.Keys)
            {
                //Intotal = Slaves[item].State.Io.NetIn + Intotal;
                outTotal = Slaves[item].State.Io.NetOut + outTotal;
            }


            SlaveList list = new SlaveList();

            foreach (var item in Slaves.Keys)
            {
                list = Slaves[item];
                Intotal = list.State.Io.NetIn + Intotal;    
            }





            Models.Index model = new Models.Index
            {
                NetInTotal = Intotal,
                NetOutTotal = outTotal,
                DiskCapacityTotal = 0,
                AvailableTotal = 0,
                TotalOnlineUsers = 0,
                TotalOnlineConnections = 0,
                Slaves = Slaves,
            };

            if (model == null) return;
            
            if (model.Slaves == null) return;
            var newState = new Cache.Models.Index { NetInTotal = model.NetInTotal , NetOutTotal = model.NetOutTotal ,DiskCapacityTotal=model.DiskCapacityTotal, AvailableTotal=model.AvailableTotal
            , TotalOnlineUsers = model.TotalOnlineUsers, TotalOnlineConnections = model.TotalOnlineConnections,Slaves= model.Slaves};

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
