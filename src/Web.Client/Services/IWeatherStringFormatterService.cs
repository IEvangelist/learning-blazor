// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public interface IWeatherStringFormatterService<out T> : IFormatProvider, ICustomFormatter
{
}
