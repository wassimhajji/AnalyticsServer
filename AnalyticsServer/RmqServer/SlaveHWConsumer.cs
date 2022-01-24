using AnalyticsServer.Cache;
using AnalyticsServer.Channels;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections;
using System.Text;
using System.Threading.Channels;

namespace AnalyticsServer.RmqServer
{
    public class SlaveHWConsumer : BackgroundService
    {
         
        private readonly string exchangeName = "SlaveExchange";
        private readonly string queueName = "HWConsumer";
        private readonly string topicKey = "Slave.HW.Key";
        
        

       

        private void ListenForHardwearEvents(CancellationToken stoppingToken,[FromServices] Channel<HWModel> _channel)
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
                            var message = JsonConvert.DeserializeObject<HWModel>(body);
                            if (message == null) return;
                            ServerCache.UpdateServerHardwear(message);
                            _channel.Writer.WriteAsync(message); 
                            //_channel.Writer.TryWrite(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
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

        private object HWChannel<T>()
        {
            throw new NotImplementedException();
        }

        private async Task ListenForUsersEventsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }

        protected  async Task ExecuteAsync(CancellationToken stoppingToken, [FromServices] Channel<HWModel> _channel)
        {
            
            ListenForHardwearEvents(stoppingToken, _channel);
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

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(5000, stoppingToken);
            }
        }
        public async Task SendMessage(Channel<HWModel> _channel)
        {
            var message = await _channel.Reader.ReadAsync();


            Console.WriteLine(message);
        }
    }

}
 