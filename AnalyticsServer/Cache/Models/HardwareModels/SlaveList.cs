using AnalyticsServer.MessagesModels;

namespace AnalyticsServer.Cache.Models
{
    public class SlaveList
    {
        public string SlaveId { get; set; }
        public luck State { get; set; }
        public StreamsWorking Streams { get; set; } 
        public UsersConnection UsersConnection { get; set; }    
    }
}
