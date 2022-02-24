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
                if(model.SlaveId == stream.Key)
                {
                    foreach (var item in stream.Value.State)
                    {
                        if (item.Time != null) streamsWorking.Working++;
                        else streamsWorking.NotWorking ++;
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
            if(string.IsNullOrWhiteSpace(model.SlaveId)) return;    
            var newState = new SlaveList { /*SlaveId = model.SlaveId,*/ SlaveInfo = model, UsersInfo = userPerSlave , Streams = streamsWorking };


            
            if(ServersList.TryGetValue(model.SlaveId, out var state))
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
                    //var str = disk.Size;
                    if (disk.Size.Contains('.'))
                    {
                        disk.Size.Replace('.', ',');
                        for (int i = 0; i < disk.Size.Length; i++)
                        {
                            
                            if (char.IsLetter(disk.Size[i]))
                            {
                                if (disk.Size[i] == 'G')
                                {
                                    
                                    Console.WriteLine($"the string from index issss {disk.Size.Remove(i, 1)}");
                                    //var num = float.Parse(str.Remove(i, 1));
                                    var str = disk.Size.Remove(i, 1).Replace('.', ',');
                                    Console.WriteLine($"the string from index is {str}");
                                    var num = decimal.Parse(str);
                                    x = num ;
                                    qSize.Enqueue(num);
                                }
                                if (disk.Size[i] == 'M')
                                {
                                    disk.Size.Replace('.', ',');
                                    //str.Remove(i-1,1);
                                    Console.WriteLine($"the string from index issss {disk.Size.Remove(i, 1)}");
                                    var num = decimal.Parse(disk.Size.Remove(i, 1));
                                    qSize.Enqueue(num);
                                    x = num ;
                                    sum = num + x;
                                }
                            }
                            
                        }
                    }
                    if (disk.Available.Contains('.'))
                    {
                        disk.Available.Replace('.', ',');
                        for (int i = 0; i < disk.Available.Length; i++)
                        {

                            if (char.IsLetter(disk.Available[i]))
                            {
                                if (disk.Available[i] == 'G')
                                {

                                    Console.WriteLine($"the string from index issss {disk.Available.Remove(i, 1)}");
                                    //var num = float.Parse(str.Remove(i, 1));
                                    var strr = disk.Available.Remove(i, 1).Replace('.', ',');
                                    Console.WriteLine($"the string from index is {strr}");
                                    var numm = decimal.Parse(strr);
                                    x = numm;
                                    qAvailable.Enqueue(numm);
                                }
                                if (disk.Available[i] == 'M')
                                {
                                    disk.Available.Replace('.', '2');
                                    //str.Remove(i-1,1);
                                    Console.WriteLine($"the string from index issss {disk.Available.Remove(i, 1)}");
                                    var numm = decimal.Parse(disk.Available.Remove(i, 1));
                                    qAvailable.Enqueue(numm);
                                    
                                }
                            }

                        }
                    }
                    if (!disk.Size.Contains('.'))
                    {


                        for (int i = 0; i < disk.Size.Length; i++)
                        {

                            if (char.IsLetter(disk.Size[i]))
                            {
                                if (disk.Size[i] == 'G')
                                {
                                    //str.Remove(i-1,1);
                                    Console.WriteLine($"the string from index is {disk.Size.Remove(i, 1)}");
                                    
                                    var num = decimal.Parse(disk.Size.Remove(i, 1));
                                    qSize.Enqueue(num);
                                   // sum = num + x;
                                }
                                if (disk.Size[i] == 'M')
                                {
                                    //str.Remove(i-1,1);
                                    Console.WriteLine($"the string from index is {disk.Size.Remove(i, 1)}");
                                    var num = decimal.Parse(disk.Size.Remove(i, 1));
                                    qSize.Enqueue(num);
                                    x = num;
                                   // sum = num + x;
                                }
                                
                            }
                            
                        }
                    }
                    if (!disk.Available.Contains('.'))
                    {
                        
                        for (int i = 0; i < disk.Available.Length; i++)
                        {

                            if (char.IsLetter(disk.Available[i]))
                            {
                                if (disk.Available[i] == 'G')
                                {

                                    Console.WriteLine($"the string from index issss {disk.Available.Remove(i, 1)}");
                                    //var num = float.Parse(str.Remove(i, 1));
                                    var strr = disk.Available.Remove(i, 1).Replace('.', ',');
                                    Console.WriteLine($"the string from index is {strr}");
                                    var numm = decimal.Parse(strr);
                                    x = numm;
                                    qAvailable.Enqueue(numm);
                                }
                                if (disk.Available[i] == 'M')
                                {
                                    disk.Available.Replace('.', ',');
                                    //str.Remove(i-1,1);
                                    Console.WriteLine($"the string from index issss {disk.Available.Remove(i, 1)}");
                                    var numm = decimal.Parse(disk.Available.Remove(i, 1));
                                    qAvailable.Enqueue(numm);

                                }
                            }

                        }
                    }
                }
                Console.WriteLine(qSize);
                decimal summ = 0;
                foreach (decimal num in qSize)
                {
                    summ = summ + num;
                }
                decimal sumAv = 0;
                foreach (var numm in qAvailable)
                {
                    sumAv = sumAv + numm;
                }
                index.NetInTotal += slave.Value.SlaveInfo.State.Io.NetIn;
                index.NetOutTotal += slave.Value.SlaveInfo.State.Io.NetOut;
                index.TotalOnlineUsers += slave.Value.UsersInfo.OnlineUsers;
                index.TotalOnlineConnections += slave.Value.UsersInfo.OnlineConnections;
                index.DiskCapacityTotal = summ;
                index.AvailableTotal = sumAv;
            }

            
            
            index.Slaves = ServersList;

            return index;
        }

        
    }
}
