using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Threading.Channels;

namespace AnalyticsServer.DbHostedServices
{
    public class StreamsDb : BackgroundService
    {
        private readonly ChannelReader<StreamMessages> _channelReader;

        private MessagesDb _db;

        public StreamsDb(Channel<StreamMessages> channel, IServiceProvider serviceProvider)
        {
            _channelReader = channel.Reader;

            _db = serviceProvider.CreateScope().ServiceProvider.GetRequiredService<MessagesDb>();
        }

        private void ReadAndSaveStreams(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {
                    var msg = await _channelReader.ReadAsync(stoppingToken);
                    //Console.WriteLine(msg.SlaveId);
                    _ = Task.Run(async () =>
                    {
                        foreach (var item in msg.State)
                        {
                            MessagesDatabase.Stream stream = new MessagesDatabase.Stream
                            {
                                Id = Guid.NewGuid(),
                                SlaveId = msg.SlaveId,
                                TimeAdded = DateTime.Now,
                                StreamId = item.StreamId,
                                state = item.state,
                                CurrentSource = item.CurrentSource,
                                VideoBitrate = item.VideoBitrate,
                                AudioBitrate = item.AudioBitrate,
                                VideoCodec = item.VideoCodec,
                                AudioCodec = item.AudioCodec,
                                Time = item.Time,
                                Width = item.Width,
                                Height = item.Height,
                                Fps = item.Fps,
                                Speed = item.Speed,
                            };

                            await _db.Streams.AddAsync(stream);

                            try
                            {
                                await _db.SaveChangesAsync();
                            }
                            catch (Exception ex)
                            {

                                Console.WriteLine(ex);
                            }

                        }

                    });                }
            });
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ReadAndSaveStreams(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
