namespace AnalyticsServer.MessagesModels
{
    public class HWMessage
    {
        public string? message { get; set; } 
        public List<HardwareMessage>? hardware { get; set; } 

        public class HardwareMessage
        {
            public long Total { get; set; }
            public long Used { get; set; }
            public long Cache { get; set; }
            public long Swap { get; set; }
            public long Boot { get; set; }
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
}
