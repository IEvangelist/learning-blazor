// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

using System.Net.Http;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class WeatherComponent
    {
        private Coordinates _coordinates = null!;
        private WeatherDetails _weatherDetails = null!;
        private WeatherWordingAndIcon _weather = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await JavaScript.GetCoordinatesAsync(
                    this,
                    nameof(OnCoordinatesPermitted),
                    nameof(OnErrorRequestingCooridnates));
            }
        }

        [JSInvokable]
        public async Task OnCoordinatesPermitted(
            decimal longitude, decimal latitude)
        {
            _coordinates = new(latitude, longitude);
            //await Http. // TODO: call Azure Function -- Weather

            await Task.CompletedTask;

            _weatherDetails = new();
            _weather = new()
            {
                Icon = "01d",
                Description = "Sunny"
            };

            await InvokeAsync(StateHasChanged);
        }

        [JSInvokable]
        public async Task OnErrorRequestingCooridnates(
            int code, string message)
        {
            await Task.CompletedTask;
        }
    }
}
