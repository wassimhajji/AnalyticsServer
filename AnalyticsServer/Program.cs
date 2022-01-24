using AnalyticsServer.Cache.Models;
using AnalyticsServer.Channels;
using AnalyticsServer.DbHostedServices;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHostedService<SlaveHWConsumer>();
//builder.Services.AddHostedService<StreamsConsumer>();
builder.Services.AddSingleton(Channel.CreateUnbounded<HWModel>());

//builder.Services.AddHostedService<HWDbStore>();

  


//var connectionString = builder.Configuration.GetConnectionString("AppDb");
//builder.Services.AddDbContext<MessagesDb>(x => x.UseSqlServer(connectionString));

var app = builder.Build();


app.MapControllers();





app.Run();
