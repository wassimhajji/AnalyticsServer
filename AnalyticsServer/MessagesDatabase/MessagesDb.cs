using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace AnalyticsServer.MessagesDatabase
{
    public class MessagesDb : DbContext
    {
        public MessagesDb(DbContextOptions options) : base(options) { }

        public DbSet<HWModel>? Hardware { get; set; }

          
        

       /* protected override void OnModelCreating (ModelBuilder builder)
        {

            base.OnModelCreating(builder);
            builder.Entity<HWModel>();


            builder.Entity<HWModel>()
                    .HasData(
                    new HWModel
                    {
                        Total = 



                    });

        }*/
    }
}
