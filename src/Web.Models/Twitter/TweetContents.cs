// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models;

public record class TweetContents(
    long Id,
    bool IsOffTopic,
    string AuthorName,
    string AuthorURL,
    string HTML,
    string URL,
    string ProviderURL,
    double Width,
    double Height,
    string Version,
    string Type,
    string CacheAge);
