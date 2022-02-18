using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class GroupingByCountryCache
    {
        private static ConcurrentDictionary<string, int> CountryGrouping = new();
        private static CountryGroupingModel country = new CountryGroupingModel();  
        public static void UpdateGroupingByCountry(ConcurrentDictionary<string, int> model)
        {
            CountryGrouping = model;
        }
        public static void UpdateGroupingByCountry1(CountryGroupingModel model)
        {
            country = model;
        }
        public static ConcurrentDictionary<string, int> GetAllCountryGroupings()
        {
            return CountryGrouping;
        }

        public static CountryGroupingModel GetAllCountryGroupings1()
        {
            return country;
        }
    }
}
