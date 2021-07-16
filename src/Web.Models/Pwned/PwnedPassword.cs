// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Learning.Blazor.Models
{
    public record class PwnedPassword(
        [property: JsonPropertyName("password")] string? Password,
        [property: JsonPropertyName("isPwned")] bool IsPwned = false,
        [property: JsonPropertyName("pwnedCount")] int PwnedCount = -1,
        [property: JsonPropertyName("hashedPassword")] string? HashedPassword = default)
    {
        [JsonIgnore]
        internal bool IsInvalid => Password is null or { Length: 0 };
    }
}
