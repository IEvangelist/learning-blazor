// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.Extensions.Localization;

namespace Learning.Blazor.Components
{
    public partial class IntroductionComponent
    {
        private LocalizedString? _intro => localize["ThankYou"];
    }
}
