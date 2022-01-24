// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models;

namespace Learning.Blazor.CosmosData;

public class Todo : Item
{
    public string UserEmail { get; set; } = null!;

    public string Title { get; set; } = null!;

    public bool IsCompleted { get; set; }

    public DateOnly? DueDate { get; set; }

    public static explicit operator TodoItem(Todo todo) =>
        new()
        {
            Id = todo.Id,
            Type = todo.Type,
            PartitionKey = ((IItem)todo).PartitionKey,
            UserEmail = todo.UserEmail,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted,
            DueDate = todo.DueDate
        };

    public static explicit operator Todo(TodoItem item) =>
        new()
        {
            Id = item.Id,
            Type = item.Type,
            UserEmail = item.UserEmail,
            Title = item.Title,
            IsCompleted = item.IsCompleted,
            DueDate = item.DueDate
        };
}
