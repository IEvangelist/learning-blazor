// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.LocalStorage;

public record Bit(bool IsSet)
{
    public static implicit operator Bit(bool isSet) =>
        new(isSet);
}
