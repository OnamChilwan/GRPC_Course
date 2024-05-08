using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyServer.Services;

namespace MyServer;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcReflectionService();
            endpoints.MapGrpcService<UnaryCalculatorProtoService>();
            endpoints.MapGrpcService<ServerStreamPrimeNumbersProtoService>();
            endpoints.MapGrpcService<ClientStreamingComputeProtoService>();
            endpoints.MapGrpcService<BiStreamingProtoService>();
            endpoints.MapControllers();
        });
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddGrpc();
        services.AddGrpcReflection();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    }
}