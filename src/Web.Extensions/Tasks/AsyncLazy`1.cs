// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Runtime.CompilerServices;

namespace Learning.Blazor.Extensions.Tasks;

public sealed class AsyncLazy<T>
{
    private readonly Lazy<Task<T>> _innerLazyTask;

    public AsyncLazy(
        Func<T> valueFactory) =>
        _innerLazyTask = new Lazy<Task<T>>(
            () => Task.Run(valueFactory));

    public AsyncLazy(Func<Task<T>> taskFactory) =>
        _innerLazyTask = new Lazy<Task<T>>(
            () => Task.Run(taskFactory));

    public TaskAwaiter<T> GetAwaiter() =>
        _innerLazyTask.Value.GetAwaiter();
}
