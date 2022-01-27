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

//var connectionString = builder.Configuration.GetConnectionString("Server=(localdb)\\mssqllocaldb;Database=StatsDatabase;Trusted_Connection=True;MultipleActiveResultSets=true");
//Console.WriteLine(connectionString);
builder.Services.AddDbContext<MessagesDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});


var app = builder.Build();


app.MapControllers();





app.Run();
 