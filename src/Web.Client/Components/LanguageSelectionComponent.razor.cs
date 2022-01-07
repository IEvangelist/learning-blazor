// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public partial class LanguageSelectionComponent
    {
        private IDictionary<CultureInfo, AzureCulture> _supportedCultures = null!;
        private CultureInfo _selectedCulture = null!;
        private ModalComponent _modal = null!;

        [Inject] HttpClient Http { get; set; } = null!;
        [Inject] public NavigationManager Navigation { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            var azureCultures =
                await Http.GetFromJsonAsync<AzureTranslationCultures>(
                    "api/cultures/all",
                    DefaultJsonSerialization.Options);

            _supportedCultures =
                Culture.MapClientSupportedCultures(azureCultures?.Translation);
        }

        private static string ToDisplayName(
            KeyValuePair<CultureInfo, AzureCulture> culturePair)
        {
            var (culture, azureCulture) = culturePair;
            return $"{azureCulture.Name} ({culture.Name})";
        }

        private Task ShowAsync() => _modal.ShowAsync();

        private async Task ConfirmAsync()
        {
            var forceRefresh =
                _selectedCulture is not null &&
                _selectedCulture != Culture.CurrentCulture;

            if (forceRefresh)
            {
                await LocalStorage.SetAsync(
                    StorageKeys.ClientCulture, _selectedCulture!.Name);
            }

            await _modal.ConfirmAsync();

            if (forceRefresh)
            {
                Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
            }
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning(
                "User '{Reason}' the language selector modal.",
                reason);
    }
}
