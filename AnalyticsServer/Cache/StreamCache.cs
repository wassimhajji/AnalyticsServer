using AnalyticsServer.Cache.Models;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class StreamCache
    {
        private static ConcurrentDictionary<string, MessagesModels.StreamMessages> Streams = new();
        private static StreamUpdate streamUpdate = new StreamUpdate();
        internal static void UpdateServerStream(StreamMessages model)
        {
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId.ToString())) return;
            if (model.State == null) return;
            var newState = new StreamMessages { SlaveId = model.SlaveId, State = model.State};
            

            if ( Streams.TryGetValue(model.SlaveId, out var state))
            {
                foreach (var item in state.State)
                {
                    Streams.TryUpdate(model.SlaveId, newState, state);
                    
                    return;
                }
                //Streams.TryUpdate(model.SlaveId, newState , state);
                //return;
            }
            Streams.TryAdd(model.SlaveId, newState);
            
            
        }
        public static ConcurrentDictionary<string, MessagesModels.StreamMessages> GetAllStreams()
        {
            return Streams;
            
            
        }
        public static StreamUpdate GetStreams(int Id)
        {
            StreamMessages str = new StreamMessages();
            //StreamMessages str1 = new StreamMessages();
            StreamUpdate str2 = new StreamUpdate();
             str = Streams.Values.FirstOrDefault();
            //return str1 = Streams.Values.FirstOrDefault();
            //return str2 = Streams.Values.FirstOrDefault();
            foreach (var item in str.State)
            {
                if (item.StreamId == Id)
                {
                    str2.SlaveId = str.SlaveId;
                    str2.StreamId = item.StreamId;
                    str2.state = item.state;
                    str2.CurrentSource = item.CurrentSource;    
                    str2.VideoBitrate = item.VideoBitrate;  
                    str2.AudioBitrate = item.AudioBitrate;  
                    str2.VideoCodec = item.VideoCodec;  
                    str2.AudioCodec = item.AudioCodec;  
                    str2.Time = item.Time;  
                    str2.Width = item.Width;
                    str2.Height = item.Height;  
                    str2.Fps = item.Fps;    
                    str2.Speed = item.Speed;    
                }
            }
            return str2;
        }
    }
}
