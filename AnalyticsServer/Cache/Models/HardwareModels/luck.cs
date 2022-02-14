using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.Cache.Models
{
    public class luck
    {
       public Cpu Cpu { get; set; }
       public Ram Ram { get; set; }
        public Io Io { get; set; }
        public List<Cache.Models.Disk> Disk { get; set; }
    }
}
