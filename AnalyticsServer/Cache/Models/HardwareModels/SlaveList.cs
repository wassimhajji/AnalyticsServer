using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class SlaveList
    {
        public string SlaveId { get; set; }
        public SlaveState State { get; set; }
        public StreamsWorking Streams { get; set; } 
        public UsersConnections UsersConnections { get; set; }    
    }
}
