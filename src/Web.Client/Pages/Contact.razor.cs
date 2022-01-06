// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Contact
    {
        private record RecognitionBuffer(int TranscriptLength, int HitCount);

        private EditContext _editContext;
        private ContactComponentModel _model = new();
        private InputText _emailInput = null!;
        private InputText _firstNameInput = null!;
        private VerificationModalComponent _modalComponent = null!;
        private RecognitionBuffer _recognitionBuffer = new(0, 0);
        private bool _isEmailReadonly = false;
        private bool _isSent;        

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (User is { Identity: { IsAuthenticated: true } })
            {
                _model = _model with
                {
                    EmailAddress = User.GetFirstEmailAddress()
                };

                _isEmailReadonly = _model.EmailAddress is not null
                    && RegexEmailAddressAttribute.EmailExpression.IsMatch(
                        _model.EmailAddress);

                _editContext = new(_model);
            }
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

        private Task OnSpeechRecognizedAsync((string Transcript, bool IsFinal) result) =>
            InvokeAsync(() =>
            {
                var (transcript, isFinal) = result;
                if (_model.Message is not null)
                {
                    var (length, _) = _recognitionBuffer;
                    var index = _model.Message.Length - length;
                    _model.Message =
                        $"{_model.Message.Remove(index).Trim()} {transcript}".Trim();
                }
                else
                {
                    _model.Message = transcript;
                }

                _recognitionBuffer =
                    isFinal
                        ? new(0, 0)
                        : _recognitionBuffer with
                        {
                            TranscriptLength = transcript.Length,
                            HitCount = _recognitionBuffer.HitCount + 1
                        };

                StateHasChanged();
            });

        private static string GetValidityCss<T>(
            EditContext context,
            Expression<Func<T?>> accessor)
        {
            var css = context?.FieldCssClass(accessor);
            return GetValidityCss(
                IsValid(css),
                IsInvalid(css),
                IsModified(css));
        }

        private static string GetValidityCss<TOne, TTwo>(
            EditContext context,
            Expression<Func<TOne?>> accessorOne,
            Expression<Func<TTwo?>> accessorTwo)
        {
            var cssOne = context?.FieldCssClass(accessorOne);
            var cssTwo = context?.FieldCssClass(accessorTwo);
            return GetValidityCss(
                IsValid(cssOne) && IsValid(cssTwo),
                IsInvalid(cssOne) || IsInvalid(cssTwo),
                IsModified(cssOne) && IsModified(cssTwo));
        }

        private static string GetValidityCss(
            bool isValid, bool isInvalid, bool isModified) =>
            (isValid, isInvalid) switch
            {
                (true, false) when isModified => "fa-check-circle has-text-success",
                (false, true) when isModified => "fa-times-circle has-text-danger",

                _ => "fa-question-circle"
            };

        private static bool IsValid(string? css) =>
            IsContainingClass(css, "valid") && !IsInvalid(css);

        private static bool IsInvalid(string? css) =>
            IsContainingClass(css, "invalid");

        private static bool IsModified(string? css) =>
            IsContainingClass(css, "modified");

        private static bool IsContainingClass(string? css, string name) =>
            css?.Contains(name, StringComparison.OrdinalIgnoreCase) ?? false;

        private async ValueTask OnValidSubmitAsync(EditContext context) =>
            await _modalComponent.PromptAsync(context);

        private async Task OnVerificationAttempted((bool isVerified, object? state) tuple)
        {
            try
            {
                var (isVerified, state) = tuple;
                if (isVerified && state is EditContext context)
                {
                    var client =
                        HttpFactory.CreateClient(HttpClientNames.ServerApi);

                    using var response =
                        await client.PostAsJsonAsync<ContactRequest>(
                        "api/contact",
                        new(
                            _model.FirstName!,
                            _model.LastName!,
                            _model.EmailAddress!,
                            _model.Subject!,
                            _model.Message!),
                        DefaultJsonSerialization.Options);

                    if (response.IsSuccessStatusCode)
                    {
                        _isSent = true;
                        context.MarkAsUnmodified();
                    }
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
