using AnalyticsServer.Cache.Models;
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
        }

        public static ConcurrentDictionary<string, ServerState> GetAllServers()
        {
            return Servers;
        }

        public static StreamsWorking StreamsWorkingCalculator(StreamsWorking model)
        {
            int notWorking = 0;
            int working = 0;
            var stream = StreamCache.GetAllStreams();
            foreach (var item in stream.Values)
            {
                foreach (var i in item.State)
                {
                    if (i.Time == null) notWorking ++;    
                    if (i.Time != null) working ++;
                    
                }
            }
            model.NotWorking = notWorking;
            model.Working = working;
            return model;
        }
        public static void General(HWModel model, StreamsWorking StreamModel)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            if (model.State == null) return;
            
            var newState = new ServerState { SlaveId = model.SlaveId, Hardwear = model };

            //var streams = StreamsWorkingCalculator(StreamModel);

            if (Servers.TryGetValue(model.SlaveId, out var state))
            {
                Servers.TryUpdate(model.SlaveId, newState, state);
                return;

            }

            Servers.TryAdd(model.SlaveId, newState);
        }

    }
}
