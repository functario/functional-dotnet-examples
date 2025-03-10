﻿using Microsoft.Extensions.Hosting;

namespace Alexandria.Local.IntegrationTests;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAspire(
        this IServiceCollection services,
        HostBuilderContext _
    )
    {
        services.AddScoped<AspireEnvironment>();
        return services;
    }
}
