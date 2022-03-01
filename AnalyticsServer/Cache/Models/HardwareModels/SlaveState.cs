namespace AnalyticsServer.Cache.Models.HardwareModels
{
    
   
    
    
    public class SlaveState
    {
        public cpu? Cpu { get; set; }
        public Ram? Ram { get; set; }
        public Io?  Io { get; set; }
        public List<Disk>? Disk { get; set; }
    }
}
