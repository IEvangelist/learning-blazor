// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class WeatherComponent
    {
        private Coordinates _coordinates = null!;
        private WeatherComponentModel? _model = null!;
        private ComponentState _state;

        [Inject]
        public IWeatherStringFormatterService<WeatherComponent> Formatter { get; set; } = null!;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        [Inject]
        internal CultureService CultureService { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await TryGetClientCoordinates();
            }
        }

        private async Task TryGetClientCoordinates() =>
            await JavaScript.GetCoordinatesAsync(
                this,
                nameof(OnCoordinatesPermitted),
                nameof(OnErrorRequestingCooridnates));

        [JSInvokable]
        public async Task OnCoordinatesPermitted(
            decimal longitude, decimal latitude)
        {
            _coordinates = new(latitude, longitude);

            var lang = CultureService.CurrentCulture.TwoLetterISOLanguageName;
            var unit = CultureService.MeasurementSystem;

            WeatherRequest request = new()
            {
                Language = lang,
                Latitude = latitude,
                Longitude = longitude,
                Units = unit
            };

            using var response = await Http.PostAsJsonAsync("api/weather/latest", request);
            var weatherDetails = await response.Content.ReadFromJsonAsync<WeatherDetails?>();
            if (weatherDetails is not null)
            {
                _model = new(weatherDetails);
                _state = ComponentState.Loaded;
            }
            else
            {
                _state = ComponentState.Error;
            }

            await InvokeAsync(StateHasChanged);
        }

        [JSInvokable]
        public async Task OnErrorRequestingCooridnates(
            int code, string message)
        {
            await Task.CompletedTask;

            _state = ComponentState.Error;
        }
    }
}
