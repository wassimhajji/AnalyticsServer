using AnalyticsServer.Cache;
using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Threading.Channels;

namespace AnalyticsServer.HostedServices
{
    public class GeneralUpdater : BackgroundService
    {
        private readonly ChannelReader<StreamMessages> _channelReader;
        private readonly ChannelReader<HWModel> channelReader;

        public GeneralUpdater(Channel<StreamMessages> channel, Channel<HWModel> _channel)
        {
            _channelReader = channel.Reader;
            channelReader = _channel.Reader;
        }

        private void getHardwareAndStreams(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                while (await _channelReader.WaitToReadAsync(stoppingToken))
                {

                    var HWMsg = await channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the general Hardware cache is{HWMsg}");
                    var StreamMsg = await _channelReader.ReadAsync(stoppingToken);
                    Console.WriteLine($"the general stream cache{StreamMsg}");

                    
                    
                    int notWorking = 0;
                    int working = 0;
                    foreach (var item in StreamMsg.State)
                    {
                        if (item.Time == null) notWorking++;
                        if (item.Time != null) working++;
                    }
                    Cache.Models.StreamsWorking model = new Cache.Models.StreamsWorking
                    {
                        NotWorking = notWorking,
                        Working = working,
                    };
                    GeneralState obj = new GeneralState
                    {
                        Hardwear = HWMsg,
                        Streams = model
                    };

                    
                   

                    
                    
                    Cache.Models.GeneralInfo generalInfo = new Cache.Models.GeneralInfo
                    {
                        NetInTotal =   HWMsg.State.Io.NetIn,
                        NetOutTotal =  HWMsg.State.Io.NetOut,
                        DiskCapacityTotal = HWMsg.State.Disks[0].Size,
                        AvailableTotal = HWMsg.State.Disks[0].Available,
                    };
                    SlaveState objFinal = new SlaveState
                    {
                        GeneralInfo = generalInfo,
                        GeneralState = obj,
                        
                    };
                    //objFinal.GeneralState.Add(obj);
                    GeneralCache.UpdateGeneral(objFinal);






                }  
                
            });
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            getHardwareAndStreams(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
