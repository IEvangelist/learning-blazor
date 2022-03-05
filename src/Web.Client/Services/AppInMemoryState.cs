// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public class AppInMemoryState
{
    private readonly IJSInProcessRuntime _javaScript;
    private string? _frameworkDescription;
    private ClientVoicePreference _clientVoicePreference =
        new("Auto", 1);
    private bool? _isDarkTheme;

    public AppInMemoryState(IJSInProcessRuntime javaScript) =>
        _javaScript = javaScript;

    public string? FrameworkDescription
    {
        get => _frameworkDescription;
        set
        {
            _frameworkDescription = value;
            AppStateChanged();
        }
    }

    public ClientVoicePreference ClientVoicePreference
    {
        get => _clientVoicePreference;
        set
        {
            _clientVoicePreference = value ?? new("Auto", 1);
            AppStateChanged();
        }
    }

    public bool IsDarkTheme
    {
        get
        {
            if (_isDarkTheme is null)
            {
                _isDarkTheme = _javaScript.GetItem<bool>(StorageKeys.PrefersDarkTheme);
            }

            return _isDarkTheme.GetValueOrDefault();
        }
        set
        {
            _javaScript.SetItem(StorageKeys.PrefersDarkTheme, value);
            _isDarkTheme = value;

            AppStateChanged();
        }
    }

    public Action<IList<Alert>>? WeatherAlertRecieved { get; set; }

    public event Action? StateChanged;

    private void AppStateChanged() => StateChanged?.Invoke();
}
