// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Models;

namespace Learning.Blazor.JokeServices;

public interface IJokeFactory
{
    Task<(string, JokeSourceDetails)> GetRandomJokeAsync();
}
