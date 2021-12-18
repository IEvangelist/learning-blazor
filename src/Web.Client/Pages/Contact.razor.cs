// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.Models.Requests;

namespace Learning.Blazor.Pages
{
    public sealed partial class Contact
    {
        private ContactComponentModel _model = new();
        private EditContext? _editContext;
        private InputText _emailInput = null!;
        private VerificationModalComponent _modalComponent = null!;
        private bool _isFormInvalid;
        private bool _isSendDisabled = true;
        private bool _isSent;        

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        protected override void OnInitialized()
        {
            if (User is { Identity: { IsAuthenticated: true } })
            {
                _model = _model with
                {
                    EmailAddress = User.GetFirstEmailAddress()
                };
            }

            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender && _emailInput is { Element: { } })
            {
                await (_emailInput?.Element?.FocusAsync(preventScroll: true)
                    ?? ValueTask.CompletedTask);
            }
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
            _isSendDisabled = _isFormInvalid;

            StateHasChanged();
        }

        private async ValueTask OnValidSubmitAsync(EditContext _) =>
            await _modalComponent.PromptAsync();

        private async Task OnVerificationAttempted(bool isVerified)
        {
            try
            {
                if (isVerified)
                {
                    var client = HttpFactory.CreateClient(HttpClientNames.ServerApi);
                    using var response = await client.PostAsJsonAsync<ContactRequest>(
                        "api/contact",
                        new(
                            _model.FirstName!,
                            _model.LastName!,
                            _model.EmailAddress!,
                            _model.Subject!,
                            _model.Message!),
                        DefaultJsonSerialization.Options);

                    _isSent = response.IsSuccessStatusCode;

                }
            }
            catch (Exception ex)
            {
                _ = ex;
                _isSent = false;
            }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
