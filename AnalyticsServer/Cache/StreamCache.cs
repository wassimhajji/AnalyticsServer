using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class StreamCache
    {
        private static ConcurrentDictionary<string, Models.StreamState> Streams = new();
        internal static void UpdateServerStreams(StreamMessages model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            if (model.Stream == null) return;
            var newState = new Models.StreamState { SlaveId = model.SlaveId,  = model };
        }
    }
}
