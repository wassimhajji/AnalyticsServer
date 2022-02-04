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
                        MessagesDatabase.Stream stream = new MessagesDatabase.Stream
                        {
                            Id = Guid.NewGuid(),
                            SlaveId = msg.SlaveId,
                            TimeAdded = DateTime.Now,
                            StreamId = msg.State[0].StreamId,
                            state = msg.State[0].state,
                            CurrentSource = msg.State[0].CurrentSource,
                            VideoBitrate = msg.State[0].VideoBitrate,
                            AudioBitrate = msg.State[0].AudioBitrate,
                            VideoCodec = msg.State[0].VideoCodec,
                            AudioCodec = msg.State[0].AudioCodec,
                            Time = msg.State[0].Time,
                            Width = msg.State[0].Width,
                            Height = msg.State[0].Height,
                            Fps = msg.State[0].Fps,
                            Speed = msg.State[0].Speed,
                        };

                         MessagesDatabase.Stream streamSecond = new MessagesDatabase.Stream
                        {
                            Id = Guid.NewGuid(),
                            SlaveId = msg.SlaveId,
                            TimeAdded = DateTime.Now,
                            StreamId = msg.State[1].StreamId,
                            state = msg.State[1].state,
                            CurrentSource = msg.State[1].CurrentSource,
                            VideoBitrate = msg.State[1].VideoBitrate,
                            AudioBitrate = msg.State[1].AudioBitrate,
                            VideoCodec = msg.State[1].VideoCodec,
                            AudioCodec = msg.State[1].AudioCodec,
                            Time = msg.State[1].Time,
                            Width = msg.State[1].Width,
                            Height = msg.State[1].Height,
                            Fps = msg.State[1].Fps,
                            Speed = msg.State[1].Speed,
                        };

                        MessagesDatabase.Stream streamThird = new MessagesDatabase.Stream
                        {
                            Id = Guid.NewGuid(),
                            SlaveId = msg.SlaveId,
                            TimeAdded = DateTime.Now,
                            StreamId = msg.State[2].StreamId,
                            state = msg.State[2].state,
                            CurrentSource = msg.State[2].CurrentSource,
                            VideoBitrate = msg.State[2].VideoBitrate,
                            AudioBitrate = msg.State[2].AudioBitrate,
                            VideoCodec = msg.State[2].VideoCodec,
                            AudioCodec = msg.State[2].AudioCodec,
                            Time = msg.State[2].Time,
                            Width = msg.State[2].Width,
                            Height = msg.State[2].Height,
                            Fps = msg.State[2].Fps,
                            Speed = msg.State[2].Speed,
                        };
                        await _db.Streams.AddAsync(stream);
                        await _db.Streams.AddAsync(streamSecond);
                        await _db.Streams.AddAsync(streamThird);
                    });
                   






                    /* foreach (var item in msg.State)
                    {
                        stream.Id = Guid.NewGuid();
                        stream.SlaveId = msg.SlaveId;
                        stream.StreamId = msg.State[1].StreamId;
                        stream.state = item.state;
                        stream.CurrentSource = item.CurrentSource;
                        stream.VideoBitrate = item.VideoBitrate;    
                        stream.AudioBitrate = item.AudioBitrate;    
                        stream.VideoCodec = item.VideoCodec;    
                        stream.AudioCodec = item.AudioCodec;   
                        stream.Time = item.Time;    
                        stream.Width = item.Width;  
                        stream.Height = item.Height;    
                        stream.Fps = item.Fps;  
                        stream.Speed = item.Speed;  
                    }*/
                   

                    try
                    {
                        await _db.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex);
                    }
                        
                    
                    foreach (var item in msg.State)
                    {
                        Console.WriteLine(item.StreamId);
                    }
                }
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
