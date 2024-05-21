// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public class LocalizableComponentBase<T> : AppComponentBase
{
    [Inject]
    public CultureService Culture { get; set; } = null!;

    [Inject]
    public ILogger<T> Logger { get; set; } = null!;

    [Inject]
    public IJSInProcessRuntime JavaScript { get; set; } = null!;

    [Inject]
    public ILocalStorageService LocalStorage { get; set; } = null!;

    [Inject]
    public AppInMemoryState AppState { get; set; } = null!;

    /// <summary>
    /// Gets the localized content for the current subcomponent,
    /// relying on the contextually appropriate <see cref="IStringLocalizer{T}"/> implementation.
    /// </summary>
    /// <remarks>
    /// This is intended to be used from within HTML templates.
    /// Example: <c>&lt;h1&gt;@Localizer["Important Title"]&lt;/h1&gt;</c>
    /// </remarks>
    [Inject]
    internal CoalescingStringLocalizer<T> Localizer { get; set; } = null!;

    [CascadingParameter]
    private Task<AuthenticationState> AuthenticationStateTask { get; set; } = null!;

    /// <summary>
    /// Gets the contextual user. This property is only
    /// available in subclasses after a call to:
    /// <code language="csharp">
    /// base.OnInitializedAsync();
    /// </code>
    /// </summary>
    public ClaimsPrincipal User { get; private set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        if (AuthenticationStateTask is not null)
        {
            var authState = await AuthenticationStateTask;
            if (authState is not null)
            {
                User = authState.User;
            }
        }
    }
}
