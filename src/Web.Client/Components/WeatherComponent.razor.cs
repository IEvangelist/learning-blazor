// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class WeatherComponent : IDisposable
    {
        private Coordinates _coordinates = null!;
        private GeoCode? _geoCode = null!;
        private WeatherComponentModel? _model = null!;
        private ComponentState _state = ComponentState.Loading;
        private bool? _isGeoLocationPermissionGranted = null;

        private readonly PeriodicTimer _timer = new(TimeSpan.FromMinutes(10));

        [Inject]
        public IWeatherStringFormatterService Formatter { get; set; } = null!;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        [Inject]
        public GeoLocationService GeoLocationService { get; set; } = null!;

        protected override Task OnInitializedAsync() =>
            TryGetClientCoordinatesAsync();

        private async Task TryGetClientCoordinatesAsync() =>
            await JavaScript.GetCoordinatesAsync(
                this,
                nameof(OnCoordinatesPermittedAsync),
                nameof(OnErrorRequestingCoordinatesAsync));

        [JSInvokable]
        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Trimming",
            "IL2026:Methods annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
            Justification = "Not an issue here.")]
        public async Task OnCoordinatesPermittedAsync(
            decimal longitude, decimal latitude)
        {
            _isGeoLocationPermissionGranted = true;
            _coordinates = new(latitude, longitude);

            try
            {
                var lang = Culture.CurrentCulture.TwoLetterISOLanguageName;
                var unit = Culture.MeasurementSystem;

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

                var latestWeatherTask =
                    GetLatestWeatherAsync(weatherRequest);

                var getGeoCodeTask = GetGeoCodeAsync(
                    longitude, latitude, requestLanguage);

                await Task.WhenAll(latestWeatherTask, getGeoCodeTask);

                var weatherDetails = latestWeatherTask.Result;
                if (weatherDetails is not null && _geoCode is not null)
                {
                    _model = new WeatherComponentModel(
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
            finally
            {
                await InvokeAsync(StateHasChanged);
            }

            if (await _timer.WaitForNextTickAsync())
            {
                await OnCoordinatesPermittedAsync(
                    _coordinates.Longitude, _coordinates.Latitude);
            }
        }

        private async Task<WeatherDetails?> GetLatestWeatherAsync(
            WeatherRequest weatherRequest)
        {
            using var response =
                await Http.PostAsJsonAsync("api/weather/latest",
                    weatherRequest,
                    DefaultJsonSerialization.Options);

            var weatherDetails =
                await response.Content.ReadFromJsonAsync<WeatherDetails>(
                    DefaultJsonSerialization.Options);

            return weatherDetails;
        }

        private async Task GetGeoCodeAsync(
            decimal longitude, decimal latitude, string requestLanguage)
        {
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
        }

        [JSInvokable]
        public async Task OnErrorRequestingCoordinatesAsync(
            int code, string message)
        {
            Logger.LogWarning(
                "The user did not grant permission to geolocation: ({Code}) {Msg}",
                code, message);

            // 1 is PERMISSION_DENIED, error codes greater than 1 are unrelated errors.
            if (code > 1)
            {
                _isGeoLocationPermissionGranted = false;
            }
            _state = ComponentState.Error;

            await InvokeAsync(StateHasChanged);
        }

        void IDisposable.Dispose() => _timer?.Dispose();
    }
}
