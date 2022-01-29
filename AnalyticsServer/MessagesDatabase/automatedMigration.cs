using Microsoft.EntityFrameworkCore;

namespace AnalyticsServer.MessagesDatabase
{
    public class automatedMigration
    {
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, MessagesDb context)
        {
            // migrate any database changes on startup (includes initial db creation)
            context.Database.Migrate();


        }
    }

}
