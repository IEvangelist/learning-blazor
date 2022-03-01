// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

internal class NativeLocalStorageWebApi
{
    const string LocalStorage = "localStorage";

    /// <summary>
    /// The fully qualified method name of the
    /// corresponding: <code>localStorage.clear();</code>
    /// value for JavaScript interop.
    /// See: <a href="https://developer.mozilla.org/docs/Web/API/Storage/clear"></a>
    /// </summary>
    internal const string Clear = $"{LocalStorage}.clear";

    /// <summary>
    /// The fully qualified method name of the
    /// corresponding: <code>localStorage.removeItem(DOMString: keyName);</code>
    /// value for JavaScript interop.
    /// See: <a href="https://developer.mozilla.org/docs/Web/API/Storage/removeItem"></a>
    /// </summary>
    internal const string RemoveItem = $"{LocalStorage}.removeItem";

    /// <summary>
    /// The fully qualified method name of the
    /// corresponding: <code>localStorage.setItem(DOMString: keyName, DOMString keyValue);</code>
    /// value for JavaScript interop.
    /// See: <a href="https://developer.mozilla.org/docs/Web/API/Storage/setItem"></a>
    /// </summary>
    internal const string SetItem = $"{LocalStorage}.setItem";

    /// <summary>
    /// The fully qualified method name of the
    /// corresponding: <code>localStorage.getItem(DOMString: keyName);</code>
    /// value for JavaScript interop.
    /// See: <a href="https://developer.mozilla.org/docs/Web/API/Storage/getItem"></a>
    /// </summary>
    internal const string GetItem = $"{LocalStorage}.getItem";
}
