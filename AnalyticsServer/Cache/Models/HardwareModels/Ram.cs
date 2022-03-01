namespace AnalyticsServer.Cache.Models.HardwareModels
{
    public class Ram
    {
        public int Total { get; set; }
        public int Used { get; set; }
        public int Cache { get; set; }
        public int Swap { get; set; }
        public int Boot { get; set; }
    }
}
