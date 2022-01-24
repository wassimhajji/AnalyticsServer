using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class StreamState
    {
        public int StreamId { get; set; }    
        public StreamModel? stream { get; set; }
    }
}
