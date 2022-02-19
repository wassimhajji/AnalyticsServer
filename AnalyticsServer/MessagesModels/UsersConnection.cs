namespace AnalyticsServer.MessagesModels
{
    public class UsersConnection
    {
        public int NbUsers { get; set; }
        public int NbConnections { get; set; }
        public int UnusedSessions { get; set; } 
    }
}
