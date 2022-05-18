// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Contact
    {
        private EditContext _editContext = null!;
        private ContactComponentModel _model = new();
        private InputText _emailInput = null!;
        private InputText _firstNameInput = null!;
        private VerificationModalComponent _modalComponent = null!;
        private bool _isEmailReadonly = false;
        private bool _isMessageReadonly = false;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            // Initializes the "User" instance.
            await base.OnInitializedAsync();

            InitializeModelAndContext();
        }

        private void InitializeModelAndContext()
        {
            if (User is { Identity.IsAuthenticated: true })
            {
                _model = _model with { EmailAddress = User.GetFirstEmailAddress() };
                _isEmailReadonly = _model.EmailAddress is not null
                    && RegexEmailAddressAttribute.EmailExpression.IsMatch(
                        _model.EmailAddress);
            }

            _editContext = new(_model);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var input = _isEmailReadonly ? _firstNameInput : _emailInput;
                await (input?.Element?.FocusAsync(preventScroll: true)
                    ?? ValueTask.CompletedTask);
            }
        }

        private void OnRecognitionStarted() => _isMessageReadonly = true;

        private void OnRecognitionStopped(SpeechRecognitionErrorEvent? error) =>
            _isMessageReadonly = false;

        private void OnSpeechRecognized(string transcript)
        {
            _model.Message = _model.Message switch
            {
                null => transcript,
                _ => $"{_model.Message.Trim()} {transcript}".Trim()
            };

            _editContext.NotifyFieldChanged(
                _editContext.Field(nameof(_model.Message)));
        }
        
        private Task OnValidSubmitAsync(EditContext _) =>
            _modalComponent.PromptAsync();

        private async Task OnVerificationAttempted((bool IsVerified, object? State) attempt)
        {
            if (attempt.IsVerified)
            {
                var client =
                    HttpFactory.CreateClient(HttpClientNames.ServerApi);

                using var response =
                    await client.PostAsJsonAsync<ContactRequest>(
                    "api/contact",
                    _model,
                    DefaultJsonSerialization.Options);

                if (response.IsSuccessStatusCode)
                {
                    AppState?.ContactPageSubmitted?.Invoke(_model);
                    _model = new();
                    InitializeModelAndContext();
                }
            }
        }
    }
}
