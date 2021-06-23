// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;

namespace Learning.Blazor.Models
{
    public abstract record Joke(JokeKind Kind)
    {
        public abstract string Text { get; }
    }

    public record ProgrammingJoke(
        string Setup,
        string Punchline) : Joke(JokeKind.TwoPart)
    {
        public override string Text =>
            $"{Setup}{Environment.NewLine}{Punchline}";
    }
}
