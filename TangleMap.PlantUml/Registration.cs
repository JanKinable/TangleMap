using Microsoft.Extensions.DependencyInjection;
using TangleMap.Model;

namespace TangleMap.PlantUml;
public static class Registration
{
    public static IServiceCollection AddPlantUml(this IServiceCollection services)
    {
        services.AddSingleton<IGraphModelRenderer, GraphModelRenderer>();

        return services;
    }
}
