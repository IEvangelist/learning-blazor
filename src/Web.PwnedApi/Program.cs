// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

var builder = WebApplication.CreateBuilder(args).AddPwnedEndpoints();
await using var app = builder.Build().MapPwnedEndpoints();
await app.RunAsync();
