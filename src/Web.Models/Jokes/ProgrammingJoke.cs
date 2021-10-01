// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Learning.Blazor.Models;

public record ProgrammingJoke(
    string Setup,
    string Punchline)
{
    public string Text =>
        $"{Setup}{Environment.NewLine}{Punchline}";
}
