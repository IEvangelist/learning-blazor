// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record JokeResponse(
        [property: JsonPropertyName("joke")] string Joke,
        [property: JsonPropertyName("details")] JokeSourceDetails Details)
    {
        public static implicit operator JokeResponse(
            (string Text, JokeSourceDetails Details) joke) =>
                new(joke.Text, joke.Details);
    }
}
