using AnalyticsServer.MessagesModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading.Channels;

namespace AnalyticsServer.RmqServer
{
    public class VodConsumer : BackgroundService
    {
        private readonly string exchangeName = "SlaveExchange";
        private readonly string queueName = "VodConsumer";
        private readonly string topicKey = "Slave.vod.Key";
        private readonly ChannelWriter<VodMessage> _channelWriter;

        public VodConsumer(Channel<VodMessage> _channel)
        {
            _channelWriter = _channel.Writer;
        }


        private void ListenForVodEvents (CancellationToken stoppingToken)
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
                           var message = JsonConvert.DeserializeObject<VodMessage>(body);
                           if (message == null) return;
                           _channelWriter.WriteAsync(message);
                           
                           
                           Console.WriteLine($"the vod message is {message} ");

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
                   ListenForVodEvents(stoppingToken); ;
               }
           }, stoppingToken);
        }

        private IConnection GetConnection()
        {
            var factory = new ConnectionFactory();
            factory.Uri = new Uri("amqp://WQK56CEwyKn7Dgpp:Mev9LAc9uP8AK8FK@20.188.60.149:5672");
            return factory.CreateConnection();
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenForVodEvents(stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);

            }
        }
    }
}
