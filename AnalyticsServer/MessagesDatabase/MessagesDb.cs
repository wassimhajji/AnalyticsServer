using AnalyticsServer.DbHostedServices;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using AnalyticsServer.Authentication;

namespace AnalyticsServer.MessagesDatabase
{
    public class MessagesDb : DbContext
    {
        public MessagesDb(DbContextOptions options) : base(options) { }

        public DbSet<Hardware>? Hardware { get; set; }
        public DbSet<HardwareDisks>? HardwareDisks { get; set; }
        public DbSet<Stream>? Streams { get; set; }
        public DbSet<Vod>? Vod { get; set; }
        public DbSet<UsersConnectionModel>? UsersConnection { get; set; }
        public DbSet<StreamGrouping>? StreamsGrouping { get; set; }
        public DbSet<CountryGroupingModel>? CountryGrouping { get; set; }   
       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HardwareDisks>()
                .HasKey(o => new { o.Id, o.TimeAdded });

            modelBuilder.Entity<Stream>()
                .HasKey(o => new { o.Id, o.SlaveId, o.StreamId });
            modelBuilder.Entity<UsersConnectionModel>()
                .HasKey(o => new { o.Id });
        }
       

        public DbSet<AnalyticsServer.Authentication.UserDto> UserDto { get; set; }
    }
}
