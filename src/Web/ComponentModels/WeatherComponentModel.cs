// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Learning.Blazor.Services;

namespace Learning.Blazor.ComponentModels
{
    public class WeatherComponentModel<T>
    {
        private readonly WeatherDetails _weatherDetails;
        private readonly CurrentWeather _currentWeather;
        private readonly DailyWeather _todaysDaily;
        private readonly WeatherWordingAndIcon _weather;

        private readonly IWeatherStringFormatterService<T> _weatherStringFormatter;

        public string ImagePath => $"media/weather/{_weather?.ToImageName() ?? "01"}.png";
        public string Main => _weather.Main;
        public string Description => _weather.Description;
        public string Temperature => string.Format(_weatherStringFormatter, "{0:t}", _currentWeather.Temperature);
        public string FeelsLike => string.Format(_weatherStringFormatter, "{0:t}", _currentWeather.FeelsLike);
        public string HighTemp => string.Format(_weatherStringFormatter, "{0:t}", _todaysDaily.Temperature.Max);
        public string LowTemp => string.Format(_weatherStringFormatter, "{0:t}", _todaysDaily.Temperature.Min);
        public MeasurementSystem MeasurementSystem => _weatherDetails.MeasurementSystem;
        public double WindSpeed => _currentWeather.WindSpeed;
        public int WindDegree => _currentWeather.WindDegree;

        public WeatherComponentModel(
            WeatherDetails weatherDetails,
            IWeatherStringFormatterService<T> weatherStringFormatter)
        {
            _weatherDetails = weatherDetails;
            _weather = _weatherDetails.Current.Weather[0];
            _currentWeather = _weatherDetails.Current;
            _todaysDaily = _weatherDetails.Daily[0];
            _weatherStringFormatter = weatherStringFormatter;
        }
    }
}
