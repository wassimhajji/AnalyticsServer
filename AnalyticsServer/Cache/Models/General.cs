using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.Cache.Models
{
    public class General
    {
        public General GeneralInfo { get; set; }
        public List<HWModel> SlaveState { get; set; }  

    }
}
