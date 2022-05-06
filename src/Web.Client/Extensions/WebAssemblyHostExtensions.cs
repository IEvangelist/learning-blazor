// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

internal static class WebAssemblyHostExtensions
{
    internal static void TrySetDefaultCulture(this WebAssemblyHost host)
    {
        try
        {
            var cache = host.Services.GetRequiredService<IMemoryCache>();
            _ = cache.Set("Test", 7);

            var localStorage = host.Services.GetRequiredService<ILocalStorageService>();
            var clientCulture = localStorage.GetItem<string>(StorageKeys.ClientCulture);
            clientCulture ??= "en-US";

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
