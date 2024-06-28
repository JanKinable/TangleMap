using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TangleMap.Model;

namespace TangleMap.Mermaid;
public static class Registration
{
    public static IServiceCollection AddMermaid(this IServiceCollection services, IConfiguration configuration)
    {
        var options = new MermaidOptions();
        configuration.GetSection("Mermaid").Bind(options);

        services.AddSingleton(options);
        services.AddSingleton<IGraphModelRenderer, GraphModelRenderer>();

        return services;
    }
}
