namespace AnalyticsServer.Cache.Models.HardwareModels
{
    public class DiskCpuTotal
    {
        public int NetInTotal { get; set; }
        public int NetOutTotal { get; set; }
        public float? DiskCapacityTotal { get; set; }
        public int? AvailableTotal { get; set; }
    }
}
