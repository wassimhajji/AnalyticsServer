using System.ComponentModel.DataAnnotations;

namespace AnalyticsServer.MessagesDatabase
{
    public class Hardware
    {
        [Key]
        public Guid Id { get; set; }
        public string SlaveId { get; set; }
        public DateTime TimeAdded { get; set; } 
        public double CpuUser { get; set; }
        public double CpuNice { get; set; }
        public double CpuSys { get; set; }
        public double CpuIoWait { get; set; }
        public double CpuIRQ { get; set; }
        public double CpuSoft { get; set; }
        public double CpuSteal { get; set; }
        public double CpuGuest { get; set; }
        public double CpuGnice { get; set; }
        public double CpuIdle { get; set; }
        public int RamTotal { get; set; }
        public int RamUsed { get; set; }
        public int RamCache { get; set; }
        public int RamSwap { get; set; }
        public int RamBoot { get; set; }
        public int IONetIn { get; set; }
        public int IONetOut { get; set; }
        public string IOTime { get; set; }
        public int IODiskRead { get; set; }
        public int IODiskWrite { get; set; }
        

    }
}
