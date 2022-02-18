// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api;

public sealed partial class Startup
{
    readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration) =>
        _configuration = configuration;
}
