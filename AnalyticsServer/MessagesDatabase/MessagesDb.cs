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
        public DbSet<Stream>? Streams { get; set; }
        public DbSet<Vod> Vods { get; set; }
        public DbSet<Existant> ExistantList   { get; set; }    
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HardwareDisks>()
                .HasKey(o => new { o.SlaveId, o.FileSystem });

            modelBuilder.Entity<Stream>()
                .HasKey(o => new { o.Id, o.SlaveId, o.StreamId });

            modelBuilder.Entity<Existant>()
                .HasKey(o => new { o.ExistantId, o.ExistantListId });

        }




    }
}
