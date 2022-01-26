using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.Cache
{
    public class ServerCache
    {
        private static ConcurrentDictionary<string, ServerState> Servers = new();
        public static void UpdateServerHardwear(HWModel model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            if (model.State == null) return;
            var newState = new ServerState { SlaveId = model.SlaveId, Hardwear = model };

            if (Servers.TryGetValue(model.SlaveId, out var state))
            {
                Servers.TryUpdate(model.SlaveId, newState, state);
                return;
            }

            Servers.TryAdd(model.SlaveId, newState);
        }

        public static ConcurrentDictionary<string, ServerState> GetAllServers()
        {
            return Servers;
        }


    }
}
