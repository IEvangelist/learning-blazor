// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LogicAppServices.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddLogicAppClient(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<LogicAppClient>();
        services.Configure<LogicAppOptions>(configuration);

        return services;
    }
}
