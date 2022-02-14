namespace AnalyticsServer.Cache.Models
{
    public class Cpu
    {
        public double User { get; set; }
        public double Nice { get; set; }
        public double Sys { get; set; }
        public double IoWait { get; set; }
        public double IRQ { get; set; }
        public double Soft { get; set; }
        public double Steal { get; set; }
        public double Guest { get; set; }
        public double Gnice { get; set; }
        public double Idle { get; set; }
    }
}
