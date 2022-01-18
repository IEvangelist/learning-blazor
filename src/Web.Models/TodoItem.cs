// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public record TodoItem(
    string Id,
    string Type,
    string PartitionKey,
    string UserEmail,
    string Title,
    bool IsComplete,
    DateOnly? DueDate);
