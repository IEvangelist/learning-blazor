// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Extensions;
using Learning.Blazor.Models;

namespace Learning.Blazor.ComponentModels
{
    public class WeatherComponentModel
    {
        private readonly WeatherDetails _weatherDetails;
        private readonly CurrentWeather _currentWeather;
        private readonly DailyWeather _todaysDaily;
        private readonly WeatherWordingAndIcon _weather;

        public string ImagePath => $"media/weather/{_weather?.ToImageName() ?? "01"}.png";
        public string Main => _weather.Description;
        public string Description => _weather.Description;
        public double Temperature => _currentWeather.Temperature;
        public double FeelsLike => _currentWeather.FeelsLike;
        public double HighTemp => _todaysDaily.Temperature.Max;
        public double LowTemp => _todaysDaily.Temperature.Min;
        public string Wind =>
            $"{_currentWeather.WindSpeed:#} {_currentWeather.WindDegree.ToCardinal()}";
        public string WindVerbose =>
            $"{_currentWeather.WindSpeed:#} MPH to the {_currentWeather.WindDegree.ToVerboseCardinal()}";

        public WeatherComponentModel(WeatherDetails weatherDetails)
        {
            _weatherDetails = weatherDetails;
            _weather = _weatherDetails.Current.Weather[0];
            _currentWeather = _weatherDetails.Current;
            _todaysDaily = _weatherDetails.Daily[0];
        }
    }
}
