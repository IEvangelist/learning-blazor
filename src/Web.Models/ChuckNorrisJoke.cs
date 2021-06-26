// Copyright (c) 2021 David Pine. All rights reserved.
//  Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    public record ChuckNorrisJoke(
        string Type,
        Value Value);

    public record Value(int Id)
    {
        private string? _joke;

        public string? Joke
        {
            get => _joke;
            set => _joke = value?.Replace("&quot;", "\"");
        }
    }
}
