// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http.Json;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Serialization;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public sealed partial class WeatherComponent : IDisposable
    {
        private Coordinates _coordinates = null!;
        private GeoCode? _geoCode = null!;
        private WeatherComponentModel<WeatherComponent>? _model = null!;
        private ComponentState _state = ComponentState.Loading;

        private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(10));

        [Inject]
        public IWeatherStringFormatterService<WeatherComponent> Formatter { get; set; } = null!;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        [Inject]
        public GeoLocationService GeoLocationService { get; set; } = null!;

        protected override Task OnInitializedAsync() =>
            TryGetClientCoordinatesAsync();

        private async Task TryGetClientCoordinatesAsync() =>
            await JavaScript.GetCoordinatesAsync(
                this,
                nameof(OnCoordinatesPermitted),
                nameof(OnErrorRequestingCooridnates));

        [JSInvokable]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Trimming",
            "IL2026:Methods annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
            Justification = "Not an issue here.")]
        public async Task OnCoordinatesPermitted(
            decimal longitude, decimal latitude)
        {
            _coordinates = new(latitude, longitude);

            var lang = Culture.CurrentCulture.TwoLetterISOLanguageName;
            var unit = Culture.MeasurementSystem;

            try
            {
                var weatherLanguages =
                    await Http.GetFromJsonAsync<WeatherLanguage[]>(
                        "api/weather/languages",
                        WeatherLanguagesJsonSerializerContext.DefaultTypeInfo);

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
                    Units = (int)unit
                };

                using var response =
                    await Http.PostAsJsonAsync("api/weather/latest",
                        weatherRequest,
                        DefaultJsonSerialization.Options);

                var weatherDetails =
                    await response.Content.ReadFromJsonAsync<WeatherDetails>(
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

            if (await _timer.WaitForNextTickAsync())
            {
                await OnCoordinatesPermitted(
                    _coordinates.Longitude, _coordinates.Latitude);
            }
        }

        [JSInvokable]
        public async Task OnErrorRequestingCooridnates(
            int code, string message)
        {
            await Task.CompletedTask;

            _state = ComponentState.Error;
        }

        void IDisposable.Dispose() => _timer?.Dispose();
    }
}
