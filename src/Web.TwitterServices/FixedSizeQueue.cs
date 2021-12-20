// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

internal class FixedSizeQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();

    public IReadOnlyCollection<T> ReadOnly => _queue;

    public int MaxCapacity { get; private set; }

    public FixedSizeQueue(int maxCapacity) => MaxCapacity = maxCapacity;

    public void Enqueue(T obj)
    {
        _queue.Enqueue(obj);

        while (_queue.Count > MaxCapacity)
        {
            _queue.TryDequeue(out var _);
        }
    }
}
