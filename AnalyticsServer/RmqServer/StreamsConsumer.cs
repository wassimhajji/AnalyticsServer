using AnalyticsServer.Cache;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections;
using System.Text;
using System.Threading.Channels;

namespace AnalyticsServer.RmqServer
{
    public class StreamsConsumer : BackgroundService
    {
        private readonly string exchangeName = "SlaveExchange";
        private readonly string queueName = "StrmConsumer";
        private readonly string topicKey = "Slave.Stream.Key";

        private readonly ChannelWriter<StreamMessages> _channelWriter;

        public StreamsConsumer(Channel<StreamMessages> _channel)
        {
            _channelWriter = _channel.Writer;
        }

        private void ListenForStreamsEvents(CancellationToken stoppingToken)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    var connection = GetConnection();
                    var channel = connection.CreateModel();
                    var message = new object();
                    channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
                    channel.QueueDeclare(queueName, true, true, true);
                    channel.QueueBind(queueName, exchangeName, topicKey);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (sender, e) =>
                    {
                       
                        try
                        {
                            StreamMessages model = new StreamMessages();
                            
                            var body = Encoding.UTF8.GetString(e.Body.ToArray());                            
                            var message = JsonConvert.DeserializeObject<StreamMessages>(body);
                            Console.WriteLine($"the stream message is : {message}");
                            if (message == null) return;
                            StreamCache.UpdateServerStream(message);
                            // var msg = message;
                            _channelWriter.WriteAsync(message);

                            /*foreach (var item in message.State)
                            {
                                Console.WriteLine(item.CurrentSource);
                            }*/

                            //StreamCache.UpdateServerStream(message);
                           
                            //Console.WriteLine(model);
                            
                            
                            
                            
                        }
                        catch (Exception ex)
                        {

                            Console.WriteLine(ex);
                        }
                        

                    };
                    channel.BasicConsume(queueName, true, consumer);

                    while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Disconnect from rabbitmq");
                    Console.WriteLine(ex);
                    await Task.Delay(5000, stoppingToken);
                    await ListenForUsersEventsAsync(stoppingToken);
                }
            }, stoppingToken);


        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://WQK56CEwyKn7Dgpp:Mev9LAc9uP8AK8FK@20.188.60.149:5672");
            return factory.CreateConnection();
        }

        private async Task ListenForUsersEventsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
           

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenForStreamsEvents(stoppingToken);
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
