// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public record class ProgrammingJoke(
    string Setup,
    string Punchline)
{
    public string Text =>
        $"{Setup}{Environment.NewLine}{Punchline}";
}
