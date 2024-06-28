using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TangleMap.Mermaid;

namespace TangleMap;


public static class GraphModelRendererRegistrations
{
    public static IServiceCollection AddModelRenderers(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMermaid(configuration);

        return services;
    }
}

