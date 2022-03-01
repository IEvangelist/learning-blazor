// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLocalStorage(
        this IServiceCollection services)
    {
        services.AddLogging();

        services.AddSingleton<
            ILocalStorage, BrowserLocalStorage>();
        services.AddSingleton<
            ISynchronousLocalStorage, SynchronousBrowserLocalStorage>();

        return services;
    }
}
