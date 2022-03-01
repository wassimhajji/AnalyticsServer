namespace AnalyticsServer.Cache.Models.HardwareModels
{
    public class Disk
    {
        public string? FileSystem { get; set; }
        public string? Size { get; set; }
        public string? Used { get; set; }
        public string? Available { get; set; }
        public string? Use { get; set; }
        public string? MontedOn { get; set; }
    }
}
