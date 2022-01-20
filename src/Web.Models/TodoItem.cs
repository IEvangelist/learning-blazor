// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.ComponentModel.DataAnnotations;

namespace Learning.Blazor.Models;

public sealed record TodoItem
{
    public string Id { get; set; } = null!;

    public string Type { get; set; } = null!;

    public string PartitionKey { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    [Required]
    public string Title { get; set; } = null!;

    public DateOnly? DueDate { get; set; }

    public bool IsCompleted { get; set; }
}
