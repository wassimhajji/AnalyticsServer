using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class SlaveList
    {
        //public string SlaveId { get; set; }
        //public SlaveState State { get; set; }
        public string SlaveId { get; set; } 
        public cpu? Cpu { get; set; }
        public Cache.Models.HardwareModels.Io? Io { get; set; } 
        public Cache.Models.HardwareModels.Ram Ram { get; set; }
        public List<Cache.Models.HardwareModels.Disk> Disk { get; set; }

        public StreamsWorking? Streams { get; set; } 
        public UsersConnections? UsersInfo { get; set; }    
    }
}
