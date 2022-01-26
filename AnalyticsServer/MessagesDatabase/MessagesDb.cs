using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AnalyticsServer.MessagesDatabase
{
    public class MessagesDb : DbContext
    {
        public MessagesDb(DbContextOptions options) : base(options) { }

        public DbSet<Hardware>? Hardware { get; set; }

          
        

       
    }
}
