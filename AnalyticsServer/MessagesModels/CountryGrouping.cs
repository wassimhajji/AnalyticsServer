using System.Collections.Concurrent;

namespace AnalyticsServer.MessagesModels
{
    public class CountryGrouping
    {
        public ConcurrentDictionary<string, int>? CountryGroups { get; set; }
    }
}
