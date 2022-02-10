using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.Cache.Models
{
    public class GeneralState
    {
        //public string SlaveId { get; set; }
       // public GeneralInfo? generalInformation { get; set; } 
       //public GeneralInfo GeneralInfo { get; set; }
        public HWModel? Hardwear { get; set; }
        public StreamsWorking? Streams { get; set; }
    }
}
