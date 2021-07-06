// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Threading.Tasks;
using Learning.Blazor.Models;

namespace Learning.Blazor.Functions.Services
{
    public interface IWeatherService
    {
        Task<WeatherDetails?> GetWeatherAsync(
            Coordinates coordinates, string? units, string? lang);
    }
}
