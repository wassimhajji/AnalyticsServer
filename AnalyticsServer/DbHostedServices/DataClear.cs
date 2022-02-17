using AnalyticsServer.MessagesDatabase;

namespace AnalyticsServer.DbHostedServices
{
    public class DataClear : BackgroundService
    {
        private readonly MessagesDb _db;

        public DataClear( IServiceProvider serviceProvider)
        {
            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }
        private void DataRemove(CancellationToken stoppingToken)
        {
            var RollBack = DateTime.Now.AddHours(-24);
            
            var oldStreams = (from p in _db.Streams
                     where DateTime.Compare(p.TimeAdded, RollBack) < 0
                     select p);

            if (oldStreams!=null)
            {
                _db.Streams.RemoveRange(oldStreams);
                _db.SaveChanges();
            }
            var oldHardware = (from p in _db.Hardware
                     where DateTime.Compare(p.TimeAdded, RollBack) < 0
                     select p);

            if (oldHardware != null)
            {
                _db.Hardware.RemoveRange(oldHardware);
                _db.SaveChanges();
            }

            var oldHardwareDisk = (from p in _db.HardwareDisks
                               where DateTime.Compare(p.TimeAdded, RollBack) < 0
                               select p);

            if (oldHardwareDisk != null)
            {
                _db.HardwareDisks.RemoveRange(oldHardwareDisk);
                _db.SaveChanges();
            }

            var oldStreamGrouping = (from p in _db.StreamsGrouping
                               where DateTime.Compare(p.TimeAdded, RollBack) < 0
                               select p);

            if (oldStreamGrouping != null)
            {
                _db.StreamsGrouping.RemoveRange(oldStreamGrouping);
                _db.SaveChanges();
            }

            var oldUsersConnection = (from p in _db.UsersConnection
                               where DateTime.Compare(p.TimeAdded, RollBack) < 0
                               select p);

            if (oldUsersConnection != null)
            {
                _db.UsersConnection.RemoveRange(oldUsersConnection);
                _db.SaveChanges();
            }

            var oldVod = (from p in _db.Vod
                               where DateTime.Compare(p.TimeAdded, RollBack) < 0
                               select p);

            if (oldVod != null)
            {
                _db.Vod.RemoveRange(oldVod);
                _db.SaveChanges();
            }
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            DataRemove(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
