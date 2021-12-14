// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Services;

public class AppInMemoryState
{
    private string? _frameworkDescription;
    private ClientVoicePreference _clientVoicePreference =
        new("Auto", 1);
    private bool _isDarkTheme;

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
        get => _isDarkTheme;
        set
        {
            _isDarkTheme = value;
            AppStateChanged();
        }
    }

    public event Action? StateChanged;

    private void AppStateChanged() => StateChanged?.Invoke();
}
