namespace AnalyticsServer.MessagesModels
{
    public class StreamMessages
    {
        public string? SlaveId { get; set; }
        public List<StreamState>? Stream { get; set; }
    }
    public class StreamState
    {
        public int StreamId { get; set; }
        public int State { get; set; }
        public string CurrentSource { get; set; }
        public string VideoBitrate { get; set; }
        public string AudioBitrate { get; set; }
        public string VideoCodec { get; set; }
        public string AudioCodec { get; set; }
        public string Time { get; set; }
        public string Width { get; set; }
        public string Height { get; set; }
        public string Fps { get; set; }
        public string Speed { get; set; }
    }
}
