// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models
{
    public record TweetContents
    {
        public bool IsOffTopic { get; init; }
        public string AuthorName { get; init; } = null!;
        public string AuthorURL { get; init; } = null!;
        public string HTML { get; init; } = null!;
        public string URL { get; init; } = null!;
        public string ProviderURL { get; init; } = null!;
        public double Width { get; init; }
        public double Height { get; init; }
        public string Version { get; init; } = null!;
        public string Type { get; init; } = null!;
        public string CacheAge { get; init; } = null!;
    }
}
