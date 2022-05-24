// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Components.Rendering;

namespace Learning.Blazor.Pages
{
    public sealed partial class Authentication
    {
        [Parameter] public string? Action { get; set; } = null!;

        private void LocalizedLogOutFragment(RenderTreeBuilder builder) =>
            ParagraphElementWithLocalizedContent(builder, Localizer, "ProcessingLogout");

        private void LocalizedLoggedOutFragment(RenderTreeBuilder builder) =>
            ParagraphElementWithLocalizedContent(builder, Localizer, "YouAreLoggedOut");

        private RenderFragment LocalizedLogInFailedFragment(string errorMessage) =>
            ParagraphElementWithLocalizedErrorContent(errorMessage, Localizer, "ErrorLoggingInFormat");

        private RenderFragment LocalizedLogOutFailedFragment(string errorMessage) =>
            ParagraphElementWithLocalizedErrorContent(errorMessage, Localizer, "ErrorLoggingOutFormat");

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
