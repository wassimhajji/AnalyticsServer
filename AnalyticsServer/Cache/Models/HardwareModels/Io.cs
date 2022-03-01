namespace AnalyticsServer.Cache.Models.HardwareModels
{
    public class Io
    {
        public int NetIn { get; set; }
        public int NetOut { get; set; }
        public string? Time { get; set; }
        public int DiskRead { get; set; }
        public int DiskWrite { get; set; }
    }
}
