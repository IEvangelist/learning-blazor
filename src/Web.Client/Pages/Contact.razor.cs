// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Learning.Blazor.Pages
{
    public sealed partial class Contact
    {
        private ContactComponentModel _model = new();

        private EditContext? _editContext;
        private bool _isFormInvalid;
        private bool _isSendDisabled;
        private bool _isLoading = false;
        private InputText _emailInput = null!;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery]
        public string Email { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            if (User is { Identity: { } })
            {
                _model = _model with
                {
                    EmailAddress = User.GetFirstEmailAddress()
                };
            }

            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;

            if (_emailInput is { Element: { } })
            {
                await(_emailInput.Element?.FocusAsync(preventScroll: true)
                    ?? ValueTask.CompletedTask);
            }
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
            _isSendDisabled = !_isFormInvalid;

            StateHasChanged();
        }

        private void Reset()
        {
            _editContext?.MarkAsUnmodified();
        }

        private async ValueTask OnValidSubmitAsync(EditContext _)
        {
            try
            {
                //var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
                //_breaches = (await httpClient.GetFromJsonAsync<BreachHeader[]>(
                //    $"api/pwned/breaches/{_model.EmailAddress}",
                //    BreachHeadersJsonSerializerContext.DefaultTypeInfo))
                //    ?? Array.Empty<BreachHeader>();

                //_state = ComponentState.Loaded;
            }
            catch (Exception ex)
            {
            }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
