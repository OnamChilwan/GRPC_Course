using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyServer;

var config = CreateConfiguration().Build();

var builder = WebHost
    .CreateDefaultBuilder(args)
    .ConfigureKestrel(options =>
    {
        options.ListenLocalhost(
            7001, 
            o => o.Protocols = HttpProtocols.Http2);
    })
    .ConfigureServices(s =>
    {
        s.AddControllers();
        s.AddEndpointsApiExplorer();
    })
    .ConfigureAppConfiguration(app =>
    {
        app.AddConfiguration(config);
    })
    .UseStartup<Startup>();

await builder
    .Build()
    .RunAsync();

static IConfigurationBuilder CreateConfiguration()
{
    return new ConfigurationBuilder()
        //.AddJsonFile("appsettings.json")
        .AddEnvironmentVariables();
}