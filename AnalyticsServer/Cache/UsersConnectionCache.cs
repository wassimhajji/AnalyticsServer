using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class UsersConnectionCache
    {
        private static ConcurrentDictionary<string, UsersConnection> UsersConnection = new();
        public static void UpdateusersConnection(ConcurrentDictionary<string, UsersConnection> model)
        {
            string str = model.Keys.First(); 
            UsersConnection user = new UsersConnection();
            user = model.Values.First();
            if (model == null) return;
            if (string.IsNullOrWhiteSpace(str)) return;
            if (user == null) return;
            var newState = new UsersConnection { NbUsers = user.NbUsers , NbConnections = user.NbConnections, UnusedSessions = user.UnusedSessions };

            if (UsersConnection.TryGetValue(str, out var state))
            {
                UsersConnection.TryUpdate(str, newState, state);
                return;

            }

            UsersConnection.TryAdd(str, newState);
        }

        public static ConcurrentDictionary<string, UsersConnection> GetAllUsersAndConnections()
        {
            return UsersConnection;
        }
    }
}
