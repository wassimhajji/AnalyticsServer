using AnalyticsServer.MessagesDatabase;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache.Models
{
    public class IndexUpdate
    {
        public ConcurrentDictionary<int, HWModel>? Slaves { get; set; }
    }
}
