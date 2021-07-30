// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using Learning.Blazor.LocalStorage;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Learning.Blazor.Extensions
{
    internal static class WebAssemblyHostExtensions
    {
        internal static async Task TrySetDefaultCultureAsync(this WebAssemblyHost host)
        {
            var localStorage = host.Services.GetRequiredService<ILocalStorage>();
            var clientCulture = await localStorage.GetAsync<string>(StorageKeys.ClientCulture);
            clientCulture ??= "en-US";

            try
            {
                CultureInfo culture = new(clientCulture);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
            }
            catch (Exception ex) when (Debugger.IsAttached)
            {
                _ = ex;
                Debugger.Break();
            }
        }
    }
}
