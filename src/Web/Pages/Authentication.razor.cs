// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Localization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace Learning.Blazor.Pages
{
    public partial class Authentication
    {
        [Parameter] public string? Action { get; set; } = null!;

        private void LocalizedLogOutFragment(RenderTreeBuilder builder) =>
            ParagraphElementWithLocalizedContent(builder, localize, "ProcessingLogout");

        private void LocalizedLoggedOutFragment(RenderTreeBuilder builder) =>
            ParagraphElementWithLocalizedContent(builder, localize, "YouAreLoggedOut");

        private RenderFragment LocalizedLogInFailedFragment(string errorMessage) =>
            ParagraphElementWithLocalizedErrorContent(errorMessage, localize, "ErrorLoggingInFormat");

        private RenderFragment LocalizedLogOutFailedFragment(string errorMessage) =>
            ParagraphElementWithLocalizedErrorContent(errorMessage, localize, "ErrorLoggingOutFormat");

        private static void ParagraphElementWithLocalizedContent(
            RenderTreeBuilder builder,
            CoalescingStringLocalizer<Authentication> localizer,
            string resourceKey)
        {
            builder.OpenElement(0, "p");
            builder.AddContent(1, localizer[resourceKey]);
            builder.CloseElement();
        }

        private static RenderFragment ParagraphElementWithLocalizedErrorContent(
            string errorMessage,
            CoalescingStringLocalizer<Authentication> localizer,
            string resourceKey) =>
            builder =>
            {
                builder.OpenElement(0, "p");
                builder.AddContent(1, localizer[resourceKey, errorMessage]);
                builder.CloseElement();
            };
    }
}
