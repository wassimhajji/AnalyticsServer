using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class StreamCache
    {
        private static ConcurrentDictionary<string, Models.StreamState> Streams = new();
        internal static void UpdateServerStream(StreamModel model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.StreamId.ToString())) return;
            if (model.State == null) return;
            var newState = new StreamState { StreamId = model.StreamId, stream = model};  

            if ( Streams.TryGetValue(model.StreamId.ToString(), out var state))
            {
                Streams.TryUpdate(model.StreamId.ToString(), newState , state);
                return;
            }
            Streams.TryAdd(model.StreamId.ToString(), newState);   
        }
        public static ConcurrentDictionary<string, Models.StreamState> GetAllStreams()
        {
            return Streams;
        }
    }
}
