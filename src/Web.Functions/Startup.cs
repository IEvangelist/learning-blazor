// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Linq;
using Learning.Blazor.Functions;
using Learning.Blazor.Functions.Extensions;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: WebJobsStartup(typeof(Startup))]
namespace Learning.Blazor.Functions
{
    internal class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            var descriptor = builder.Services.FirstOrDefault(d => d.ServiceType == typeof(IConfiguration));
            if (descriptor?.ImplementationInstance is IConfigurationRoot configuration)
            {
                builder.Services
                    .AddOpenWeatherMapServices(configuration)
                    .BuildServiceProvider(true);
            }
            else
            {
                throw new ApplicationException("The function requires a valid IConfigurationRoot instance.");
            }
        }

        IServiceCollection ConfigureServices(
            IServiceCollection services, IConfiguration configuration) =>
            services;
    }
}
