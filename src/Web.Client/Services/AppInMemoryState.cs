// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public sealed class AppInMemoryState
{
    private readonly IStorage _localStorage;
    private string? _frameworkDescription;
    private ClientVoicePreference? _clientVoicePreference;
    private bool? _isDarkTheme;

    public AppInMemoryState(IStorage localStorage) =>
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
        get => _clientVoicePreference ??=
            _localStorage.GetItem<ClientVoicePreference>(
                StorageKeys.ClientVoice)
            ?? new("Auto", 1);
        set
        {
            _localStorage.SetItem(
                StorageKeys.ClientVoice,
                _clientVoicePreference = value ?? new("Auto", 1));

            AppStateChanged();
        }
    }

    public bool IsDarkTheme
    {
        get => _isDarkTheme ??= _localStorage.GetItem<bool>(StorageKeys.PrefersDarkTheme);
        set
        {
            _localStorage.SetItem(
                StorageKeys.PrefersDarkTheme,
                _isDarkTheme = value);

            AppStateChanged();
        }
    }

    public Action<IList<Alert>>? WeatherAlertReceived { get; set; }

    public event Action? StateChanged;

    private void AppStateChanged() => StateChanged?.Invoke();
}
