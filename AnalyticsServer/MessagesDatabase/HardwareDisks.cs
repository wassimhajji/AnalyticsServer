using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnalyticsServer.MessagesDatabase
{
    public class HardwareDisks
    {
        public string SlaveId { get; set; }
        public DateTime TimeAdded { get; set; } 
        public string FileSystem { get; set; }
        public string Size { get; set; }
        public string Used { get; set; }
        public string Available { get; set; }
        public string Use { get; set; }
        public string MontedOn { get; set; }
    }
}
