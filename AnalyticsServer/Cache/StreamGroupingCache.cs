using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class StreamGroupingCache
    {
        private static ConcurrentDictionary<string, int> StreamGrouping = new();
        public static void UpdateStreamGrouping(ConcurrentDictionary<string, int> model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.Keys.FirstOrDefault())) return;
           /* foreach (var item in model.Keys)
            {
                string key = item;
                int value = model[key];

                if (StreamGrouping.TryGetValue(model.Keys.FirstOrDefault(), out var state))
                {
                    StreamGrouping.TryUpdate(key,value, state);
                    return;

                }
                StreamGrouping.TryAdd(key,value);
            }*/
           StreamGrouping = model;
            
        }
        public static ConcurrentDictionary<string, int> GetAllStreamGroupings()
        {
            return StreamGrouping;
        }
    }
}
