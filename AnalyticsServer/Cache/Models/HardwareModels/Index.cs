using AnalyticsServer.HostedServices;
using AnalyticsServer.MessagesDatabase;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache.Models
{
    
    public class Index
    {
        public int NetInTotal { get; set; }
        public int NetOutTotal { get; set; }
        public float? DiskCapacityTotal { get; set; }
        public int? AvailableTotal { get; set; }
        public int TotalOnlineUsers { get; set; }   
        public int TotalOnlineConnections { get; set; }
       // public List<SlaveList>? Slaves { get; set; }
        public ConcurrentDictionary<string, SlaveList>? Slaves { get; set; }
    }
}
