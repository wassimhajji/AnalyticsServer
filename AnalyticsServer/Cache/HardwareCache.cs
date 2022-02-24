using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class HardwareCache
    {
        private static ConcurrentDictionary<string, Cache.Models.Index> General = new();
        
        
        public static void UpdateServerHardwear(ConcurrentDictionary<string, SlaveList> Slaves )
        {
            /*int Intotal = 0;
            int outTotal = 0;
            int size = 0;
            int dispo = 0;*/
            int users = 0;
            int connections = 0;

            /*foreach (var item in Slaves.Keys)
            {
                //Intotal = Slaves[item].State.Io.NetIn + Intotal;
                outTotal = Slaves[item].State.Io.NetOut + outTotal;
            }*/

            /*
            SlaveList list = new SlaveList();

            foreach (var item in Slaves.Keys)
            {
                list = Slaves[item];
                Intotal = list.State.Io.NetIn + Intotal;    
            }
            */
            var slaves = Cache.ServerCache.GetAllServers();
            var user = Cache.UsersConnectionCache.GetAllUsersAndConnections();
            var disk = Cache.ServerCache.GetAllServers();
            int netIn = 0;
            int netOut = 0;
            
           /* foreach (var item in Slaves.Values)
            {
                netIn = netIn + item.State.Io.NetIn;
                netOut = netOut + item.State.Io.NetOut;
            }*/
            foreach (var item in Slaves.Values)
            {
                users = users + item.UsersInfo.OnlineUsers;
                connections = connections + item.UsersInfo.OnlineConnections;
            }

            Models.Index model = new Models.Index
            {
                NetInTotal = netIn,
                NetOutTotal = netOut,
                DiskCapacityTotal = 0,
                //AvailableTotal = 0,
                TotalOnlineUsers = users,
                TotalOnlineConnections = connections,
                //Slaves = slaves,
            };

            if (model == null) return;
            
            if (model.Slaves == null) return;
            var newState = new Cache.Models.Index { NetInTotal = model.NetInTotal , NetOutTotal = model.NetOutTotal ,DiskCapacityTotal=model.DiskCapacityTotal, AvailableTotal=model.AvailableTotal
            , TotalOnlineUsers = model.TotalOnlineUsers, TotalOnlineConnections = model.TotalOnlineConnections,/*Slaves= slaves*/};

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
