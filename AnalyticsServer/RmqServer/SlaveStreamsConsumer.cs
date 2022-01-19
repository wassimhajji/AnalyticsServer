using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AnalyticsServer.RmqServer
{
    public class SlaveStreamsConsumer : BackgroundService
    {
        private readonly string exchangeName = "SlaveExchange";
        private readonly string queueName = "HWConsumer";
        private readonly Dictionary<string, object> args = new Dictionary<string, object>();
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connection = GetConnection();
            var channel = connection.CreateModel();
            var message = new object();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Topic);
            channel.QueueDeclare(queueName, true, false, true, args);
            channel.QueueBind(queueName, exchangeName, "Slave.HW.Key");

            while (!stoppingToken.IsCancellationRequested)
            {

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, e) =>
                {
                    var body = e.Body.ToArray();
                    message = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(body));
                    channel.BasicConsume(queueName, true, consumer);

                };
                consumer.Shutdown += OnConsumerShutdown;
                consumer.Registered += OnConsumerRegistered;
                consumer.Unregistered += OnConsumerUnregistered;
                consumer.ConsumerCancelled += OnConsumerConsumerCancelled;

                await Task.Delay(3000, stoppingToken);
            }

            

        }
        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://WQK56CEwyKn7Dgpp:Mev9LAc9uP8AK8FK@20.188.60.149:5672");
            return factory.CreateConnection();
        }
        private void OnConsumerConsumerCancelled(object sender, ConsumerEventArgs e) { }
        private void OnConsumerUnregistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerRegistered(object sender, ConsumerEventArgs e) { }
        private void OnConsumerShutdown(object sender, ShutdownEventArgs e) { }
        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e) { }
    } 
}
