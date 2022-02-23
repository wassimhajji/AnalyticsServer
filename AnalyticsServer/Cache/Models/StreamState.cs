using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class StreamState
    {
        public string? SlaveId { get; set; }    
        public StreamMessages? stream { get; set; }
    }
}
