using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace AnalyticsServer.Cache
{
    public class GeneralCache
    {
        private static ConcurrentDictionary<string, SlaveState> General = new();
        
        public static void UpdateGeneral(SlaveState model)
        {
            if (model == null) return;
            

            var  newState = new SlaveState { GeneralInfo = model.GeneralInfo , GeneralState = model.GeneralState  };



            if (General.TryGetValue(model.GeneralInfo.AvailableTotal, out var SlaveState))
            {
                General.TryUpdate("General State", newState, SlaveState);
                return;

            }




            General.TryAdd("General State", newState);
        }
        public static ConcurrentDictionary<string, SlaveState> GetGeneral()
        {
            return General;
        }
    }
}
