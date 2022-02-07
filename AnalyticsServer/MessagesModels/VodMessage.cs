namespace AnalyticsServer.MessagesModels
{
    public class state
    {
        public List<object> DownloadList { get; set; }
        public List<int> Existing { get; set; }
    }
    public class VodMessage
    {
        public string SlaveId { get; set; }
        public state State { get; set; }
    }
}
