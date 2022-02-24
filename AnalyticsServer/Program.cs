using AnalyticsServer.Cache.Models;
using AnalyticsServer.DbHostedServices;
using AnalyticsServer.HostedServices;
using AnalyticsServer.MessagesDatabase;
using AnalyticsServer.MessagesModels;
using AnalyticsServer.RmqServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.Filters;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<Channel<HWModel>>(_ => Channel.CreateUnbounded<HWModel>());
builder.Services.AddSingleton<Channel<StreamMessages>>(_ => Channel.CreateUnbounded<StreamMessages>());
builder.Services.AddSingleton<Channel<VodMessage>>(_ => Channel.CreateUnbounded<VodMessage>());
builder.Services.AddSingleton<Channel<ConcurrentDictionary<string,UsersConnection>>> (_ => Channel.CreateUnbounded<ConcurrentDictionary<string,UsersConnection>>());
builder.Services.AddSingleton<Channel<ConcurrentDictionary<string, int>>>(_ => Channel.CreateUnbounded<ConcurrentDictionary<string, int>>());
builder.Services.AddSingleton<Channel<CountryGrouping>>(_ => Channel.CreateUnbounded<CountryGrouping>());
builder.Services.AddHostedService<SlaveHWConsumer>();
//builder.Services.AddHostedService<IndexService>();
builder.Services.AddHostedService<StreamsConsumer>();
builder.Services.AddHostedService<UsersConnectionConsumer>();
builder.Services.AddHostedService<HWDb>();
builder.Services.AddHostedService<HWDisksDb>();
builder.Services.AddHostedService<StreamsDb>();
builder.Services.AddHostedService<VodConsumer>();
builder.Services.AddHostedService<VodDb>();
builder.Services.AddHostedService<HardwareGeneral>();
builder.Services.AddHostedService<UsersConnectionDb>();
builder.Services.AddHostedService<StreamGroupingConsumer>();
builder.Services.AddHostedService<StreamGroupingDb>();
builder.Services.AddHostedService<DataClear>();
builder.Services.AddHostedService<GroupingByCountryConsumer>();
builder.Services.AddHostedService<CountryGroupingDb>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Auhorization header using the bearer scheme(\"bearer {token}\")", 
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });



//var connectionString = builder.Configuration.GetConnectionString("Server=(localdb)\\mssqllocaldb;Database=StatsDatabase;Trusted_Connection=True;MultipleActiveResultSets=true");
//Console.WriteLine(connectionString);
builder.Services.AddDbContext<MessagesDb>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});





var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

builder.WebHost.UseUrls("http://localhost:5002");





app.Run();
 