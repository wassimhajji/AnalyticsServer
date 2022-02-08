using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.Cache
{
    public class GeneralCache
    {
        private static ConcurrentDictionary<string, GeneralState> General = new();
        public static void UpdateGeneral(HWModel model, StreamsWorking StreamModel)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            if (model.State == null) return;

            var newState = new GeneralState { SlaveId = model.SlaveId, Hardwear = model, Streams = StreamModel };

            //var streams = StreamsWorkingCalculator(StreamModel);

            if (General.TryGetValue(model.SlaveId, out var state))
            {
                General.TryUpdate(model.SlaveId, newState, state);
                return;

            }

            General.TryAdd(model.SlaveId, newState);
        }
        public static ConcurrentDictionary<string, GeneralState> GetGeneral()
        {
            return General;
        }
    }
}
