// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components;

public class AppComponentBase : ComponentBase
{
    public Task ExternalInvokeAsync() =>
        InvokeAsync(StateHasChanged);

    public Task ExternalInvokeAsync(Action<AppComponentBase> action) =>
        InvokeAsync(() =>
        {
            action?.Invoke(this);
            StateHasChanged();
        });

    public Task ExternalInvokeAsync(Func<AppComponentBase, Task>? task) =>
        InvokeAsync(async () =>
        {
            await (task?.Invoke(this) ?? Task.CompletedTask);
            StateHasChanged();
        });
}
