using RabbitMQ.Client;

namespace AnalyticsServer.RmqServer
{
    public class RmqConnection
    {
        static void Connection()
        {
            var factory = new ConnectionFactory
            {
                Uri = new Uri("amqp://WQK56CEwyKn7Dgpp:Mev9LAc9uP8AK8FK@20.188.60.149:5672")
            };
            using var Connection = factory.CreateConnection();
            using var channel = Connection.CreateModel();
            SlaveExchangeHW.Consume(channel);   
            SlaveExchangeStreams.Consume(channel);


        }
    }
}
