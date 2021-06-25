// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;

namespace Learning.Blazor.Services
{
    internal interface IJokeService
    {
        Task<string?> GetJokeAsync();
    }
}
