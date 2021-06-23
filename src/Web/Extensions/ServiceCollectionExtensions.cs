// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using Learning.Blazor.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddClientServices(
            this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(
                    nameof(services), "A service collection is required.");
            }

            return services.AddScoped<IJokeService, ProgrammingJokeService>();
        }
    }
}
