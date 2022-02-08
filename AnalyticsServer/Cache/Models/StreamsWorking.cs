namespace AnalyticsServer.Cache.Models
{
    public class StreamsWorking
    {
        public int Working { get; set; }  
        public int NotWorking { get; set; }
        public int StreamsWorkingCalculator()
        { 
            int Working = 0;    
            int NotWorking = 0; 
            var stream = StreamCache.GetAllStreams();
            foreach (var item in stream.Values)
            {
                foreach (var i in item.State)
                {
                    if (i.Time == null) NotWorking++;
                    if (i.Time != null) Working++;
                }
            }
            return Working;
            return NotWorking;
        }
    }
}
