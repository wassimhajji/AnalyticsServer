using Microsoft.EntityFrameworkCore;
using Polly;

namespace AnalyticsServer.MessagesDatabase
{
    public static IHost DatabaseUpdater<MessagesDb>(this IHost host)
        where MessagesDb : DbContext
    {
        using (var scope = host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            var context = services.GetService<MessagesDb>();

            try
            {
                var retry = Policy.Handle<Exception>().WaitAndRetry(new[]
                {
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10),
                    TimeSpan.FromSeconds(15),
                });
                retry.Execute(() =>
                {
                    context.Database.Migrate();
                });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
        }
        return host;
    }
}
