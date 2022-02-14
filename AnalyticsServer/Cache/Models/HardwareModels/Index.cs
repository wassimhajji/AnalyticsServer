using AnalyticsServer.HostedServices;
using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.Cache.Models
{
    
    public class Index
    {
        public int NetInTotal { get; set; }
        public int NetOutTotal { get; set; }
        public int? DiskCapacityTotal { get; set; }
        public int? AvailableTotal { get; set; }
        public List<SlaveList>? Slaves { get; set; }
    }
}
