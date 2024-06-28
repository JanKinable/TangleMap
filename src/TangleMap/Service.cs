using Microsoft.Extensions.Hosting;

namespace TangleMap;

public class Service : IHostedService
{
    private readonly IBootstrap _bootstrap;
    private readonly IHostApplicationLifetime _appLifetime;

    public Service(IBootstrap bootstrap,
        IHostApplicationLifetime appLifetime)
    {
        _bootstrap = bootstrap;
        _appLifetime = appLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
         await _bootstrap.RunAsync(cancellationToken);
        _appLifetime.StopApplication();   
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
