// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Abstractions.RealTime;

/// <summary>
/// A class that contains well-known SignalR group names.
/// </summary>
public static class HubGroupNames
{
    /// <summary>
    /// The group name to join (or leave) when interested in receiving live tweets.
    /// </summary>
    public const string Tweets = nameof(Tweets);
}
