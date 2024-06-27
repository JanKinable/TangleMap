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
builder.Services.AddSingleton(options);

builder.Services.AddPlugins(configuration);

