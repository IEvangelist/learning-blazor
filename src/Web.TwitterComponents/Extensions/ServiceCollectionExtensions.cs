// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddTwitterComponent(
        this IServiceCollection services,
        IConfiguration configuration) => services;
    // TODO: leave hooks in place for future additions
}
