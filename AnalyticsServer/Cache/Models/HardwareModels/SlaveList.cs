using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class SlaveList
    {
        //public string SlaveId { get; set; }
        //public SlaveState State { get; set; }
        public HWModel SlaveInfo { get; set; }
        public StreamsWorking Streams { get; set; } 
        public UsersConnections UsersInfo { get; set; }    
    }
}
