using AnalyticsServer.Cache.Models;
using AnalyticsServer.Cache.Models.HardwareModels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using System.Collections.Concurrent;

namespace AnalyticsServer.Cache
{
    public class IndexUpdate
    {
        private static ConcurrentDictionary<string, SlaveList> ServersList = new();

        public static void UpdateIndex(HWModel model)
        {

            var streams = Cache.StreamCache.GetAllStreams();
            StreamsWorking streamsWorking = new StreamsWorking();
            foreach (var stream in streams)
            {
                if (model.SlaveId == stream.Key)
                {
                    foreach (var item in stream.Value.State)
                    {
                        if (item.Time != null) streamsWorking.Working++;
                        else streamsWorking.NotWorking++;
                    }
                }
            }




            var users = Cache.UsersConnectionCache.GetAllUsersAndConnections();
            UsersConnections userPerSlave = new UsersConnections();
            foreach (var user in users)
            {
                if (model.SlaveId == user.Key)
                {
                    userPerSlave.OnlineUsers = user.Value.NbUsers;
                    userPerSlave.OnlineConnections = user.Value.NbConnections;
                };
            }


            if (model == null) return;
            if (string.IsNullOrWhiteSpace(model.SlaveId)) return;
            var newState = new SlaveList { /*SlaveId = model.SlaveId,*/ SlaveInfo = model, UsersInfo = userPerSlave, Streams = streamsWorking };



            if (ServersList.TryGetValue(model.SlaveId, out var state))
            {
                ServersList.TryUpdate(model.SlaveId, newState, state);
                return;
            }
            ServersList.TryAdd(model.SlaveId, newState);



        }

        public static ConcurrentDictionary<string, SlaveList> GetAllSlaves()
        {
            return ServersList;
        }

        public static Cache.Models.Index GetIndex()
        {
            Queue<decimal> qSize = new System.Collections.Generic.Queue<decimal>();
            Queue<decimal> qAvailable = new System.Collections.Generic.Queue<decimal>();
            decimal x = 0;
            decimal sum = 0;
            var index = new Cache.Models.Index();
            foreach (var slave in ServersList)
            {
                foreach (var disk in slave.Value.SlaveInfo.State.Disks)
                {

                    var str = disk.Size.Remove(disk.Size.Length - 1, 1);
                    var numm = decimal.Parse(str);
                    qSize.Enqueue(numm);







                    var strr = disk.Available.Remove(disk.Size.Length - 1, 1);
                    var num = decimal.Parse(str);
                    qAvailable.Enqueue(num);

                }


                Console.WriteLine(qSize);
                decimal summ = 0;
                foreach (decimal num in qSize)
                {

                    summ += num;
                }
                decimal sumAv = 0;
                foreach (var numm in qAvailable)
                {
                    sumAv += numm;
                }
                index.Slaves = ServersList;
                index.DiskCapacityTotal = summ.ToString();
                index.AvailableTotal = sumAv.ToString();
                index.NetInTotal += slave.Value.SlaveInfo.State.Io.NetIn;
                index.NetOutTotal += slave.Value.SlaveInfo.State.Io.NetOut;
                index.TotalOnlineUsers += slave.Value.UsersInfo.OnlineUsers;
                index.TotalOnlineConnections += slave.Value.UsersInfo.OnlineConnections;





               
            }
            return index;
        }

            public static string getQueue()
            {
                Queue<decimal> qSize = new System.Collections.Generic.Queue<decimal>();
                foreach (var item in ServersList)
                {
                    foreach (var disk in item.Value.SlaveInfo.State.Disks)
                    {

                        var str = disk.Size.Remove(disk.Size.Length - 1, 1);
                        var numm = decimal.Parse(str);
                        qSize.Enqueue(numm);
                    }
                }
                decimal sum = 0;
                string subresult = string.Empty;
                foreach (var item in qSize)
                {
                    sum += item;
                    subresult += $" || the number to sum is {item} ||";
                }

                string result = subresult + $" || la somme est {sum}";

                return result;
            }
        }
    }
