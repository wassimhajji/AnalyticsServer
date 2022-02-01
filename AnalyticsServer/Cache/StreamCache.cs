using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class StreamCache
    {
        private static ConcurrentDictionary<string, Models.StreamState> Streams = new();
        internal static void UpdateServerStream(StreamMessages model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId.ToString())) return;
            if (model.State == null) return;
            var newState = new StreamState { SlaveId = model.SlaveId, stream = model};  

            if ( Streams.TryGetValue(model.SlaveId, out var state))
            {
                Streams.TryUpdate(model.SlaveId, newState , state);
                return;
            }
            Streams.TryAdd(model.SlaveId, newState);   
        }
        public static ConcurrentDictionary<string, Models.StreamState> GetAllStreams()
        {
            return Streams;
        }
    }
}
