// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;

namespace Learning.Blazor.CosmosData;

public class Todo : Item
{
    public string UserEmail { get; set; } = null!;

    public string Title { get; set; } = null!;

    public bool IsComplete { get; set; }

    public DateOnly? DueDate { get; set; }

    public static implicit operator TodoItem(Todo todo) =>
        new(todo.Id,
            todo.Type,
            ((IItem)todo).PartitionKey,
            todo.UserEmail,
            todo.Title,
            todo.IsComplete,
            todo.DueDate);

    public static implicit operator Todo(TodoItem item) =>
        new()
        {
            Id = item.Id,
            Type = item.Type,
            UserEmail = item.UserEmail,
            Title = item.Title,
            IsComplete = item.IsComplete,
            DueDate = item.DueDate
        };
}
