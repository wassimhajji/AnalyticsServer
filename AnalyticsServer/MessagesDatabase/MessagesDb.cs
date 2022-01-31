using AnalyticsServer.DbHostedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AnalyticsServer.MessagesDatabase
{
    public class MessagesDb : DbContext
    {
        public MessagesDb(DbContextOptions options) : base(options) { }

        public DbSet<Hardware>? Hardware { get; set; }
        public DbSet<HardwareDisks>? HardwareDisks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HardwareDisks>()
                .HasKey(o => new { o.SlaveId, o.FileSystem });
        }




    }
}
