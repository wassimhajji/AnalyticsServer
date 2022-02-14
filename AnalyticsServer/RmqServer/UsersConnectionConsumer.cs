using AnalyticsServer.MessagesModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace AnalyticsServer.RmqServer
{
    public class UsersConnectionConsumer : BackgroundService
    {
        private readonly string exchangeName = "SessionExchange";
        private readonly string queueName = "UsersConnection";
        private readonly string topicKey = "User.Session.Stat";

        private readonly ChannelWriter<ConcurrentDictionary<string, UsersConnection>> _channelWriter;

        public UsersConnectionConsumer(Channel<ConcurrentDictionary<string, UsersConnection>> _channel)
        {
            _channelWriter = _channel.Writer;
        }

        private void ListenForUsersConnectionEvents(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    using var connection = GetConnection();
                    using var channel = connection.CreateModel();
                    var message = new object();
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
                    channel.QueueDeclare(queueName, true, true, true);
                    channel.QueueBind(queueName, exchangeName, topicKey);


                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        try
                        {
                            var body = Encoding.UTF8.GetString(e.Body.ToArray());
                            var message = JsonConvert.DeserializeObject<ConcurrentDictionary<string, UsersConnection>>(body);
                            Console.WriteLine($"the rmq users message is : {message}");
                            if (message == null) return;
                            _channelWriter.WriteAsync(message);
                            Cache.UsersConnectionCache.UpdateusersConnection(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    };

                    /*var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                        try
                        {
                            var body = Encoding.UTF8.GetString(e.Body.ToArray());
                            var message = JsonConvert.DeserializeObject<dynamic>(body);
                            if (message == null) return;
                            Console.WriteLine($"the message is {message}");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }
                    };*/
                    channel.BasicConsume(queueName, true, consumer);

                    while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Disconnect from rabbitmq");
                    Console.WriteLine(ex);
                    await Task.Delay(5000, stoppingToken);
                    ListenForUsersConnectionEvents(stoppingToken);
                }
            }, stoppingToken);
        }
           


        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenForUsersConnectionEvents(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://WQK56CEwyKn7Dgpp:Mev9LAc9uP8AK8FK@20.188.60.149:5672"); 
            return factory.CreateConnection();
        }
    }
}
