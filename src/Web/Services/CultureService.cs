// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Globalization;
using Learning.Blazor.Models;

namespace Learning.Blazor.Services
{
    public class CultureService
    {
        public CultureInfo CurrentCulture { get; } = CultureInfo.CurrentCulture;

        public RegionInfo CurrentRegion { get; } = new(CultureInfo.CurrentCulture.LCID);

        public MeasurementSystem MeasurementSystem =>
            CurrentRegion.IsMetric
                ? MeasurementSystem.Metric
                : MeasurementSystem.Imperial;
    }
}
