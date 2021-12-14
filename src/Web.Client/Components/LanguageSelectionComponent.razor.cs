// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public partial class LanguageSelectionComponent
    {
        private HashSet<(CultureInfo Culture, AzureCulture AzureCulture)>
            _supportedCultures = null!;

        private CultureInfo _selectedCulture = null!;
        private ModalComponent _modal = null!;

        [Inject] HttpClient Http { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            try
            {
                var azureCultures =
                    await Http.GetFromJsonAsync<AzureTranslationCultures>(
                        "api/cultures/all",
                        DefaultJsonSerialization.Options);

                _supportedCultures =
                    Culture.MapClientSupportedCultures(azureCultures?.Translation);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
            }
        }

        private static string ToDisplayName(
            (CultureInfo Culture, AzureCulture AzureCulture)? culturePair)
        {
            var (hasValue, (culture, azureCulture)) = culturePair;
            return hasValue
                ? $"{azureCulture.Name} ({culture.Name})"
                : "";
        }

        private async Task Show() => await _modal.Show();

        private async Task Confirm()
        {
            var forceRefresh = false;
            if (_selectedCulture is not null &&
                _selectedCulture != Culture.CurrentCulture)
            {
                forceRefresh = true;
                await LocalStorage.SetAsync(
                    StorageKeys.ClientCulture, _selectedCulture.Name);
            }

            await _modal.Confirm();

            if (forceRefresh)
            {
                Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
            }
        }

        private void OnDismissed(DismissalReason reason) =>
            Logger.LogWarning("User '{Reason}' the language selector modal.", reason);
    }
}
