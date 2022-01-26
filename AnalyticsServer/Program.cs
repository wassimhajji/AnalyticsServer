using AnalyticsServer.Cache.Models;
using AnalyticsServer.DbHostedServices;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Channels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();


builder.Services.AddSingleton<Channel<HWModel>>(_ => Channel.CreateUnbounded<HWModel>());
builder.Services.AddHostedService<SlaveHWConsumer>();
//builder.Services.AddHostedService<HWDbService>();

var connectionStrings = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<MessagesDb>(x => x.UseSqlServer(connectionStrings));

var app = builder.Build();


app.MapControllers();





app.Run();
 