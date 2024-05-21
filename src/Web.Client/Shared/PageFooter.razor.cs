// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Shared;

public sealed partial class PageFooter
{
    const string CodeUrl =
        "https://github.com/IEvangelist/learning-blazor";
    const string LicenseUrl =
        "https://github.com/IEvangelist/learning-blazor/blob/main/LICENSE";
    const string DavidPineUrl =
        "https://davidpine.net";

    private string? _frameworkDescription;

    protected override void OnInitialized() =>
        _frameworkDescription = AppState.FrameworkDescription;
}
