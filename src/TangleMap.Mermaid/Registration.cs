﻿using Microsoft.Extensions.DependencyInjection;
using TangleMap.Model;

namespace TangleMap.Mermaid;
public static class Registration
{
    public static IServiceCollection AddMermaid(this IServiceCollection services)
    {
        services.AddSingleton<IGraphModelRenderer, GraphModelRenderer>();

        return services;
    }
}
