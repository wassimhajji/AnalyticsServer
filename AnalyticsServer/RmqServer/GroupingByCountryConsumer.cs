using AnalyticsServer.MessagesModels;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace AnalyticsServer.RmqServer
{
    public class GroupingByCountryConsumer : BackgroundService
    {
        private readonly string exchangeName = "SessionExchange";
        private readonly string queueName = "GroupingCountry";
        private readonly string topicKey = "Country.Session.Stat";

        private readonly ChannelWriter<CountryGrouping> _channelWriter;

        public GroupingByCountryConsumer(Channel<CountryGrouping> channel)
        {
            _channelWriter = channel.Writer;
        }

        private void ListenForGroupingByCountryEvents(CancellationToken stoppingToken)
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
                           var message = JsonConvert.DeserializeObject< ConcurrentDictionary<string, int>>(body);
                           if (message == null) return;
                           
                           Console.WriteLine($"the Grouping by country message is : {message}");
                           Cache.GroupingByCountryCache.UpdateGroupingByCountry(message);

                           CountryGrouping countryGrouping = new CountryGrouping
                           {
                               CountryGroups = message
                           };

                           _channelWriter.WriteAsync(countryGrouping);


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
                   ListenForGroupingByCountryEvents(stoppingToken);
               }
           });
        }

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ListenForGroupingByCountryEvents(stoppingToken);
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
