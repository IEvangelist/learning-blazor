// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Timers;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using SystemTimer = System.Timers.Timer;

namespace Learning.Blazor.Components
{
    public sealed partial class WeatherComponent : IDisposable
    {
        private Coordinates _coordinates = null!;
        private GeoCode? _geoCode = null!;
        private WeatherComponentModel<WeatherComponent>? _model = null!;
        private ComponentState _state;
        private SystemTimer _timer = null!;

        [Inject]
        public IWeatherStringFormatterService<WeatherComponent> Formatter { get; set; } = null!;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        [Inject]
        public GeoLocationService GeoLocationService { get; set; } = null!;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await TryGetClientCoordinates();

                _timer = new SystemTimer
                {
                    Interval = TimeSpan.FromMinutes(10).TotalMilliseconds,
                    AutoReset = true
                };
                _timer.Elapsed += OnTimerElapsed;
                _timer.Start();
            }
        }

        private async void OnTimerElapsed(object sender, ElapsedEventArgs args)
        {
            if (_coordinates is not null)
            {
                await OnCoordinatesPermitted(_coordinates.Longitude, _coordinates.Latitude);
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

            var lang = Culture.CurrentCulture.TwoLetterISOLanguageName;
            var unit = Culture.MeasurementSystem;

            try
            {
                var weatherLanguages =
                    await Http.GetFromJsonAsync<WeatherLanguage[]>("api/weather/languages");

                var requestLanguage =
                    weatherLanguages
                        ?.FirstOrDefault(language => language.AzureCultureId == lang)
                        ?.WeatherLanguageId
                    ?? "en";

                WeatherRequest weatherRequest = new()
                {
                    Language = requestLanguage,
                    Latitude = latitude,
                    Longitude = longitude,
                    Units = unit
                };

                using var response = await Http.PostAsJsonAsync("api/weather/latest", weatherRequest);
                var weatherDetails =
                    await response.Content.ReadFromJsonAsync<WeatherDetails?>(
                        DefaultJsonSerialization.Options);

                if (_geoCode is null)
                {
                    GeoCodeRequest geoCodeRequest = new()
                    {
                        Language = requestLanguage,
                        Latitude = latitude,
                        Longitude = longitude,
                    };

                    _geoCode =
                        await GeoLocationService.GetGeoCodeAsync(geoCodeRequest);
                }

                if (weatherDetails is not null && _geoCode is not null)
                {
                    _model = new WeatherComponentModel<WeatherComponent>(
                        weatherDetails, _geoCode, Formatter);
                    _state = ComponentState.Loaded;
                }
                else
                {
                    _state = ComponentState.Error;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
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

        void IDisposable.Dispose()
        {
            if (_timer is { Enabled: true })
            {
                _timer.Stop();
                _timer.Elapsed -= OnTimerElapsed;
                _timer.Dispose();
            }
        }
    }
}
