// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.Components
{
    public partial class LanguageSelectionComponent
    {
        private IEnumerable<CultureInfo> _supportedCultures = null!;
        private CultureInfo _selectedCulture = null!;
        private ModalComponent _modal = null!;

        [Inject] NavigationManager Navigation { get; set; } = null!;
        [Inject] HttpClient Http { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var azureCultures =
                    await Http.GetFromJsonAsync<AzureTranslationCultures>(
                        "api/cultures/all",
                        DefaultJsonSerialization.Options);

                if (azureCultures is not null)
                {
                    _supportedCultures =
                        CultureInfo.GetCultures(CultureTypes.SpecificCultures)
                            .Where(culture =>
                                azureCultures.Translation.ContainsKey(
                                    culture.TwoLetterISOLanguageName));
                }
            }
            catch (Exception ex) when (Debugger.IsAttached)
            {
                Logger.LogError(ex, ex.Message, ex.StackTrace);
                Debugger.Break();
            }
        }

        private string ToDisplayName(CultureInfo? culture) =>
            culture is not null
                ? $"{culture.Name} - {culture.NativeName} ({culture.TwoLetterISOLanguageName})"
                : "";

        private async Task Show() => await _modal.Show();

        private async Task Confirm()
        {
            await LocalStorage.SetAsync(
                StorageKeys.ClientCulture, _selectedCulture.Name);

            await _modal.Confirm();

            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning("User '{Reason}' the language selector modal.", reason);
    }
}
