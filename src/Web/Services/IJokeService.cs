// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;
using System.Threading.Tasks;

namespace Learning.Blazor.Services
{
    public interface IJokeService
    {
        Task<Joke?> GetJokeAsync();
    }
}
