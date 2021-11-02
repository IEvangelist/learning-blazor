// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

var builder = WebApplication.CreateBuilder(args);
builder.AddPwnedEndpoints();

var app = builder.Build();
app.MapPwnedEndpoints();

await app.RunAsync();
