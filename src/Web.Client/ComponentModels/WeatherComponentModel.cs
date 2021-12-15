// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.ComponentModels;

public class WeatherComponentModel
{
    private readonly WeatherDetails _weatherDetails;
    private readonly CurrentWeather _currentWeather;
    private readonly DailyWeather _todaysDaily;
    private readonly WeatherWordingAndIcon _weather;
    private readonly GeoCode _geoCode;
    private readonly IWeatherStringFormatterService _weatherStringFormatter;

    public string ImagePath => $"media/weather/{_weather?.ToImageName() ?? "01"}.png";
    public string Main => _weather.Main;
    public string Description => _weather.Description;
    public string Temperature => string.Format(_weatherStringFormatter, "{0:t}", _currentWeather.Temperature);
    public string FeelsLike => string.Format(_weatherStringFormatter, "{0:t}", _currentWeather.FeelsLike);
    public string HighTemp => string.Format(_weatherStringFormatter, "{0:t}", _todaysDaily.Temperature.Max);
    public string LowTemp => string.Format(_weatherStringFormatter, "{0:t}", _todaysDaily.Temperature.Min);
    public MeasurementSystem MeasurementSystem => (MeasurementSystem)_weatherDetails.Units;
    public double WindSpeed => _currentWeather.WindSpeed;
    public int WindDegree => _currentWeather.WindDegree;
    public IReadOnlyList<DailyWeather> DailyWeather => _weatherDetails.Daily.Skip(1).ToList().AsReadOnly();
    public string? City => _geoCode?.City;
    public string? State => _geoCode?.PrincipalSubdivision;
    public string? Country => _geoCode?.CountryCode;

    public string Message =>
        $"The current weather for {City} is {Temperature}. " +
        $"You can expect {Description}, with a high of {HighTemp} and a low of {LowTemp}";

    public WeatherComponentModel(
        WeatherDetails weatherDetails,
        GeoCode geoCode,
        IWeatherStringFormatterService weatherStringFormatter)
    {
        _weatherDetails = weatherDetails;
        _weather = _weatherDetails.Current.Weather[0];
        _currentWeather = _weatherDetails.Current;
        _todaysDaily = _weatherDetails.Daily[0];
        _geoCode = geoCode;
        _weatherStringFormatter = weatherStringFormatter;
    }

    public string GetDailyFontAwesomeClass(DailyWeather daily) => daily.Weather[0].ToFontAwesomeClass();
    public string GetDailyImagePath(DailyWeather daily) => $"media/weather/{daily.Weather[0]?.ToImageName() ?? "01"}.png";
    public string GetDailyHigh(DailyWeather daily) => string.Format(_weatherStringFormatter, "{0:t}", daily.Temperature.Max);
    public string GetDailyLow(DailyWeather daily) => string.Format(_weatherStringFormatter, "{0:t}", daily.Temperature.Min);
}
