// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Options;

/// <summary>
/// See: https://haveibeenpwned.com/api/v3
/// </summary>
public class PwnedOptions
{
    public string ApiBaseAddress { get; set; } = "https://haveibeenpwned.com/api/v3/";

    public string PasswordsApiBaseAddress { get; set; } = "https://api.pwnedpasswords.com/";

    public string ApiKey { get; set; } = null!;
}
