// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Learning.Blazor.Extensions;

namespace Learning.Blazor
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            ConfigureServices(
                builder.Services,
                builder.Configuration,
                builder.HostEnvironment);

            await builder.Build().RunAsync();
        }

        static void ConfigureServices(
            IServiceCollection services,
            IConfiguration configuration,
            IWebAssemblyHostEnvironment hostEnvironment)
        {
            services.AddScoped(
                _ => new HttpClient
                {
                    BaseAddress = new Uri(hostEnvironment.BaseAddress)
                });
            services.AddJokeServices(configuration);
            services.AddTwitterComponent(configuration);
            services.AddMsalAuthentication(options =>
            {
                configuration.Bind("AzureAdB2C", options.ProviderOptions.Authentication);
            });
        }
    }
}
