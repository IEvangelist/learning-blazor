// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public record class AzureCulture
{
    public string Name { get; set; } = null!;
    public string NativeName { get; set; } = null!;
    public Direction Dir { get; set; }
}
