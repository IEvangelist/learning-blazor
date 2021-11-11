// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

[assembly: FunctionsStartup(typeof(Startup))]
namespace Learning.Blazor.Functions;

internal class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var configuration = builder.GetContext().Configuration;
        builder.Services
                .AddOpenWeatherMapServices(configuration)
                .BuildServiceProvider(true);
    }
}
