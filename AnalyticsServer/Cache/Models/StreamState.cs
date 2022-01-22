using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class StreamState
    {
        public string? StreamId { get; set; }    
        public StreamMessages? stream { get; set; }
    }
}
