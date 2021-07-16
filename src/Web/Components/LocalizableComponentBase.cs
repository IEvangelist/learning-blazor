// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.LocalStorage;
using Learning.Blazor.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace Learning.Blazor.Components
{
    public class LocalizableComponentBase<T> : ComponentBase
    {
        [Inject]
        public IStringLocalizer<T> Localizer { get; set; } = null!;

        [Inject]
        public CultureService Culture { get; set; } = null!;

        [Inject]
        public ILogger<T> Logger { get; set; } = null!;

        [Inject]
        public ILocalStorage LocalStorage { get; set; } = null!;

        [Inject]
        public IJSRuntime JavaScript { get; set; } = null!;
    }
}
