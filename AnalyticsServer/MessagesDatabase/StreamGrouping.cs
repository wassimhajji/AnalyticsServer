using System.ComponentModel.DataAnnotations;

namespace AnalyticsServer.MessagesDatabase
{
    public class StreamGrouping
    {
        [Key]
        public Guid Id { get; set; }    
        public DateTime TimeAdded { get; set; } 
        public string StreamId { get; set; }    
        public int UsersNumber { get; set; }    
    }
}
