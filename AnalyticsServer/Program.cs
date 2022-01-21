using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("AppDb");
builder.Services.AddDbContext<MessagesDb>(x => x.UseSqlServer(connectionString));

builder.Services.AddHostedService<SlaveHWConsumer>();



app.Run();
