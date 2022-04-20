// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Extensions;

internal static class EventCallbackExtensions
{
    internal static async Task TryInvokeAsync<TComponentBase>(
        this EventCallback eventCallback,
        TComponentBase? componentBase = null)
        where TComponentBase : AppComponentBase
    {
        if (eventCallback is { HasDelegate: true })
        {
            await eventCallback.InvokeAsync();
            if (componentBase is not null)
            {
                await componentBase.ExternalInvokeAsync();
            }
        }
    }

    internal static async Task TryInvokeAsync<T, TComponentBase>(
        this EventCallback<T> eventCallback,
        TComponentBase? componentBase = null)
        where TComponentBase : AppComponentBase
    {
        if (eventCallback is { HasDelegate: true })
        {
            await eventCallback.InvokeAsync();
            if (componentBase is not null)
            {
                await componentBase.ExternalInvokeAsync();
            }
        }
    }

    internal static async Task TryInvokeAsync<T, TComponentBase>(
        this EventCallback eventCallback,
        T args,
        TComponentBase? componentBase = null)
        where TComponentBase : AppComponentBase
    {
        if (eventCallback is { HasDelegate: true })
        {
            await eventCallback.InvokeAsync(args);
            if (componentBase is not null)
            {
                await componentBase.ExternalInvokeAsync();
            }
        }
    }
    
    internal static async Task TryInvokeAsync<T, TComponentBase>(
        this EventCallback<T> eventCallback,
        T args,
        TComponentBase? componentBase = null)
        where TComponentBase : AppComponentBase
    {
        if (eventCallback is { HasDelegate: true })
        {
            await eventCallback.InvokeAsync(args);
            if (componentBase is not null)
            {
                await componentBase.ExternalInvokeAsync();
            }
        }
    }
}
