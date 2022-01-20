// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models.Requests;

public sealed record CreateTodoRequest
{
    public string Title { get; set; } = null!;
    public string UserEmail { get; set; } = null!;
    public bool IsCompleted { get; set; }
    public DateOnly? DueDate { get; set; }
}
