namespace AnalyticsServer.MessagesDatabase
{
    public class StreamModel
    {
        public int StreamId { get; set; }
        public StreamStates? State { get; set; }
        public string? CurrentSource { get; set; }
        public string? VideoBitrate { get; set; }
        public string? AudioBitrate { get; set; }
        public string? VideoCodec { get; set; }
        public string? AudioCodec { get; set; }
        public string? Time { get; set; }
        public string? Width { get; set; }
        public string? Height { get; set; }
        public string? Fps { get; set; }
        public string? Speed { get; set; }
    }
    public enum StreamStates
    {
        None = 0,
        Starting = 1,
        Started = 2,
        Stopping = 3,
        Stopped = 4
    }
}
