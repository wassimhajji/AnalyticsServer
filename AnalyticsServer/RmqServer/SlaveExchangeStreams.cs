using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace AnalyticsServer.RmqServer
{
    public class SlaveExchangeStreams : DefaultBasicConsumer
    {
        public static void Consume(IModel channel)
        {
            channel.ExchangeDeclare("SlaveExchange", ExchangeType.Topic);
            channel.QueueDeclare("StreamsQueue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            channel.QueueBind("StreamsQueue", "SlaveExchange", "Slave.Stream.Key");
            channel.BasicQos(0, 100, false);
            

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (sender, e) =>
            {
                var body = e.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                
            };
        }
    }
}