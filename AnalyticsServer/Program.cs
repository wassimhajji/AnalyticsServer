using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.RmqServer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddHostedService<SlaveHWConsumer>();
builder.Services.AddHostedService<StreamsConsumer>();   


//var connectionString = builder.Configuration.GetConnectionString("AppDb");
//builder.Services.AddDbContext<MessagesDb>(x => x.UseSqlServer(connectionString));

var app = builder.Build();


app.MapControllers();





app.Run();
