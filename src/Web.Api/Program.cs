// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(
        static webBuilder => webBuilder.UseStartup<Startup>());

using var host = builder.Build();

await host.RunAsync();
