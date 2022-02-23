namespace AnalyticsServer.MessagesDatabase
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

    public class Ram
    {
        public int Total { get; set; }
        public int Used { get; set; }
        public int Cache { get; set; }
        public int Swap { get; set; }
        public int Boot { get; set; }
    }

    public class Io
    {
        public int NetIn { get; set; }
        public int NetOut { get; set; }
        public string? Time { get; set; }
        public int DiskRead { get; set; }
        public int DiskWrite { get; set; }
    }

    public class Disk
    {
        public string? FileSystem { get; set; }
        public string? Size { get; set; }
        public string? Used { get; set; }
        public string? Available { get; set; }
        public string? Use { get; set; }
        public string? MontedOn { get; set; }
    }

    public class State
    {
        public Cpu? Cpu { get; set; }
        public Ram? Ram { get; set; }
        public Io? Io { get; set; }
        public List<Disk>? Disks { get; set; }
    }

    public class HWModel
    {
        public string? SlaveId { get; set; }
        public State? State { get; set; }
    }
}
