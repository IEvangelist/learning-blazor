// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Passwords
    {
        private readonly PasswordsComponentModel _model = new();

        private EditContext? _editContext;
        private InputText _passwordInput = null!;
        private bool _isFormInvalid;
        private PwnedPassword? _pwnedPassword = null!;
        private ComponentState _state = ComponentState.Unknown;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        protected override void OnInitialized()
        {
            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await (_passwordInput?.Element?.FocusAsync(preventScroll: true) ?? ValueTask.CompletedTask);
            }
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
        }

        private void Reset()
        {
            _model.PlainTextPassword = null!;
            _pwnedPassword = null!;
            _state = ComponentState.Unknown;
            _editContext?.MarkAsUnmodified();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage(
            "Trimming",
            "IL2026:Methods annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code",
            Justification = "Not an issue here.")]
        private async ValueTask OnValidSubmitAsync(EditContext _)
        {
            try
            {
                _state = ComponentState.Loading;
                var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
                _pwnedPassword = await httpClient.GetFromJsonAsync<PwnedPassword>(
                    $"api/pwned/passwords/{_model.PlainTextPassword}",
                    DefaultJsonSerialization.Options);

                _state = ComponentState.Loaded;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                _state = ComponentState.Error;
            }
        }
    }
}
