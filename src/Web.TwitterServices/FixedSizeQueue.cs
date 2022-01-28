// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor;

internal class FixedSizeQueue<T>
{
    private readonly ConcurrentQueue<T> _queue = new();

    public IReadOnlyCollection<T> ReadOnly =>
        _queue.Take(MaxCapacity).ToList();

    public int MaxCapacity { get; }

    public FixedSizeQueue(int maxCapacity = 100)
    {
        if ((MaxCapacity = maxCapacity) <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maxCapacity),
                "The max capacity must be larger than 0.");
        }
    }

    public void Enqueue(T obj)
    {
        _queue.Enqueue(obj);

        while (_queue.Count > MaxCapacity)
        {
            _queue.TryDequeue(out var _);
        }
    }
}
