// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Breaches
    {
        private readonly BreachesComponentModel _model = new();

        private EditContext? _editContext;
        private bool _isFormInvalid;
        private InputText _emailInput = null!;
        private BreachHeader[] _breaches = Array.Empty<BreachHeader>();
        private BreachDetails? _breach = null!;
        private ComponentState _state = ComponentState.Unknown;
        private string? _filter = null!;
        private ModalComponent _modal = null!;

        private IEnumerable<BreachHeader> _filteredBreaches =>
            _filter is null
                ? _breaches
                : _breaches.Where(
                    breach => breach.Name.Contains(
                        _filter, StringComparison.OrdinalIgnoreCase));

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        [Parameter]
        [SupplyParameterFromQuery(Name = "email")]
        public string EmailAddress { get; set; } = null!;

        [CascadingParameter]
        public Error Error { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;

            if (EmailAddress is not null and { Length: > 0 })
            {
                _model.EmailAddress = EmailAddress;
                if (_editContext.Validate())
                {
                    await OnValidSubmitAsync(_editContext);
                }
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await (_emailInput?.Element?.FocusAsync() ?? ValueTask.CompletedTask);
            }
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
        }

        private void Reset()
        {
            _model.EmailAddress = EmailAddress = null!;
            _breaches = Array.Empty<BreachHeader>();
            _filter = null!;
            _state = ComponentState.Unknown;
            _editContext?.MarkAsUnmodified();
        }

        private async ValueTask OnValidSubmitAsync(EditContext _)
        {
            try
            {
                _state = ComponentState.Loading;
                var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
                _breaches = (await httpClient.GetFromJsonAsync<BreachHeader[]>(
                    $"api/pwned/breaches/{_model.EmailAddress}",
                    BreachHeadersJsonSerializerContext.DefaultTypeInfo))
                    ?? Array.Empty<BreachHeader>();

                _state = ComponentState.Loaded;
            }
            catch (Exception ex)
            {
                Error.ProcessError(ex);
                _state = ComponentState.Error;
            }
        }

        private async Task ShowAsync(string breachName)
        {
            await _modal.ShowAsync();

            var httpClient =
                HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);

            _breach =
                await httpClient.GetFromJsonAsync<BreachDetails>(
                $"api/pwned/breach/{breachName}",
                BreachDetailJsonSerializerContext.DefaultTypeInfo);
        }
    }
}
