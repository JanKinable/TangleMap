using KrokiNet.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TangleMap;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

IConfiguration configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

builder.Services.AddSingleton(configuration);

var options = CommandLine.Parser.Default.ParseArguments<Options>(args);
builder.Services.AddSingleton(options.Value);

builder.Services.AddModelRenderers();

builder.Services.AddSingleton<IProjectDependencyCollector, ProjectDependencyCollector>();
builder.Services.AddSingleton<IImageGenerator, ImageGenerator>();
builder.Services.AddSingleton<IBootstrap, Bootstrap>();

builder.Services.AddKroki();

builder.Services.AddHostedService<Service>();

using IHost host = builder.Build();

await host.RunAsync();
