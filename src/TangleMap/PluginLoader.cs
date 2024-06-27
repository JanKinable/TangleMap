using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TangleMap.Model;

namespace TangleMap;


public static class PluginRegistration
{
    public static IServiceCollection AddPlugins(this IServiceCollection services, IConfiguration configuration)
    {
        var plugIns = configuration.GetSection("Plugins").Get<List<PluginConfiguration>>() ?? [];

        foreach (var plugIn in plugIns)
        {
            var assembly = Assembly.LoadFile(plugIn.AssemblyPath!);
            foreach (var type in assembly.GetTypes())
            {
                if (typeof(IGraphModelRenderer).IsAssignableFrom(type)) //something wrong here
                {
                    IGraphModelRenderer instance = Activator.CreateInstance(type) as IGraphModelRenderer;
                    if (instance != null)
                    {
                        services.AddSingleton(instance);
                    }
                }
            }
        }

        return services;
    }
}

