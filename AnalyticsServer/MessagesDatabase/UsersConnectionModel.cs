using System.ComponentModel.DataAnnotations;

namespace AnalyticsServer.MessagesDatabase
{
    public class UsersConnectionModel
    {
        [Key]
        public Guid Id { get; set; }    
        public string? SlaveId { get; set; }
        public int UsersNumber { get; set; }
        public int ConnectionsNumber { get; set; }
        public int UnusedSessions { get; set; }
        public DateTime TimeAdded { get; set; }
    }
}
