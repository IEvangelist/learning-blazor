// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

await Host.CreateDefaultBuilder(args)
    .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
    .Build()
    .RunAsync();
