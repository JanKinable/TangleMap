using Microsoft.Extensions.DependencyInjection;
using TangleMap.Model;

namespace TangleMap.GraphViz;
public static class Registration
{
    public static IServiceCollection AddMGraphViz(this IServiceCollection services)
    {
        services.AddSingleton<IGraphModelRenderer, GraphModelRenderer>();

        return services;
    }
}
