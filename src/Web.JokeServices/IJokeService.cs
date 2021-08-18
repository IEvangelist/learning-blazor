// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Models;

namespace Learning.Blazor.JokeServices;

internal interface IJokeService
{
    JokeSourceDetails SourceDetails { get; }

    Task<string?> GetJokeAsync();
}
