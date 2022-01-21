using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections;
using System.Text;

namespace AnalyticsServer.RmqServer
{
    public class SlaveHWConsumer : BackgroundService
    {
        private readonly string exchangeName = "SlaveExchange";
        private readonly string queueName = "HWConsumer";
        private readonly string topicKey = "Slave.HW.Key";
        private readonly Dictionary<string,object> args = new Dictionary<string,object>();
        public Queue HardwareQueue = new Queue();

        private void ListenForUsersEvents(CancellationToken stoppingToken, ILogger<SlaveHWConsumer> logger, ModelBuilder builder)
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
                        var body = Encoding.UTF8.GetString(e.Body.ToArray());
                        Console.WriteLine("message: " + body);
                        var message = JsonConvert.DeserializeObject<dynamic>(body);
                        if (message == null) return;
                        if (message != null)
                        {
                            foreach (var item in message)
                            {
                                Console.WriteLine(item);
                                HardwareQueue.Enqueue(item);

                            }
                            logger.LogWarning($"server's hardware :  { HardwareQueue}  ");

                            builder.Entity<HWModel>()
                            .HasData(new HWModel
                            {
                                Total = HardwareQueue.Dequeue().ToString(),
                                Used = HardwareQueue.Dequeue().ToString(),
                                Cache = HardwareQueue.Dequeue().ToString(),
                                Swap = HardwareQueue.Dequeue().ToString(),
                                Boot = HardwareQueue.Dequeue().ToString(),
                                User = HardwareQueue.Dequeue().ToString(),
                                Nice = HardwareQueue.Dequeue().ToString(),
                                Sys = HardwareQueue.Dequeue().ToString(),
                                IoWait = HardwareQueue.Dequeue().ToString(),
                                IRQ = HardwareQueue.Dequeue().ToString(),
                                Soft = HardwareQueue.Dequeue().ToString(),
                                Steal = HardwareQueue.Dequeue().ToString(),
                                Guest = HardwareQueue.Dequeue().ToString(),
                                Gnice = HardwareQueue.Dequeue().ToString(),
                                Idle = HardwareQueue.Dequeue().ToString(),

                            });
                        }

                    };
                    channel.BasicConsume(queueName,true,consumer);
                    
                    while (!stoppingToken.IsCancellationRequested) await Task.Delay(1000, stoppingToken);
                }
                catch (Exception ex)
                {

                    Console.WriteLine("Disconnect from rabbitmq");
                    Console.WriteLine(ex);
                    await Task.Delay(5000, stoppingToken);
                    await ListenForUsersEventsAsync(stoppingToken);
                }
            },stoppingToken);
            
            
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
            await ListenForUsersEventsAsync(stoppingToken);
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
 