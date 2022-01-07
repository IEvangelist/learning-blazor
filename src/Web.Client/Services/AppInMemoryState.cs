// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public class AppInMemoryState
{
    private readonly ISynchronousLocalStorage _localStorage;
    private string? _frameworkDescription;
    private ClientVoicePreference _clientVoicePreference =
        new("Auto", 1);
    private bool? _isDarkTheme;

    public AppInMemoryState(ISynchronousLocalStorage localStorage) =>
        _localStorage = localStorage;

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
                _isDarkTheme = _localStorage.Get<Bit>(StorageKeys.PrefersDarkTheme)?.IsSet ?? false;
            }

            return _isDarkTheme.GetValueOrDefault();
        }
        set
        {
            _localStorage.Set<Bit>(StorageKeys.PrefersDarkTheme, value);
            _isDarkTheme = value;

            AppStateChanged();
        }
    }

    public Action<IList<Alert>>? WeatherAlertRecieved { get; set; }

    public event Action? StateChanged;

    private void AppStateChanged() => StateChanged?.Invoke();
}
