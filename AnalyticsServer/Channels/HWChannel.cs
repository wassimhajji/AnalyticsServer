using AnalyticsServer.MessagesDatabase;
using System.Threading.Channels;

namespace AnalyticsServer.Channels
{
    public class HWChannel
    {
       public HWChannel()
        {
            var _channel = Channel.CreateUnbounded<HWModel>(); 
        }
    }
}
