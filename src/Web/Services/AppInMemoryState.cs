namespace Learning.Blazor.Services;

public class AppInMemoryState
{
    private string? _frameworkDescription;

    public string? FrameworkDescription
    {
        get => _frameworkDescription;
        set
        {
            _frameworkDescription = value;
            AppStateChanged();
        }
    }

    public event Action? StateChanged;

    private void AppStateChanged() => StateChanged?.Invoke();
}
