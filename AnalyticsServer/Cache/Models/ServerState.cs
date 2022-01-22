using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.Cache.Models
{
    public class ServerState
    {
        public string SlaveId { get; set; }
        public HWModel? Hardwear { get; set; }
    }
}
