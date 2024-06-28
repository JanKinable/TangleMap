﻿using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TangleMap.GraphViz;
using TangleMap.Mermaid;
using TangleMap.PlantUml;

namespace TangleMap;


public static class GraphModelRendererRegistrations
{
    public static IServiceCollection AddModelRenderers(this IServiceCollection services)
    {
        services.AddMermaid();
        services.AddPlantUml();
        services.AddMGraphViz();

        return services;
    }
}

