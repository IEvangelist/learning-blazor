﻿// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System.Net.Http.Json;
using HaveIBeenPwned.Client.Models;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Components;
using Learning.Blazor.Serialization;
using Learning.Blazor.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;

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
        [SupplyParameterFromQuery]
        public string Email { get; set; } = null!;

        [CascadingParameter]
        public Error Error { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;

            if (Email is not null and { Length: > 0 })
            {
                _model.EmailAddress = Email;
                if (_editContext.Validate())
                {
                    await OnValidSubmitAsync(_editContext);
                }
            }

            await (_emailInput?.Element?.FocusAsync(preventScroll: true) ?? ValueTask.CompletedTask);
        }

        protected override async ValueTask OnLocationChangedAsync(LocationChangedEventArgs args)
        {
            if (Email is not null and { Length: > 0 } &&
                _model.EmailAddress != Email &&
                _editContext is not null)
            {
                _model.EmailAddress = Email;
                if (_editContext.Validate())
                {
                    await OnValidSubmitAsync(_editContext);
                }
            }
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
            StateHasChanged();
        }

        private void Reset()
        {
            _model.EmailAddress = Email = null!;
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
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        }

        private async Task Show(string breachName)
        {
            await _modal.Show();

            var httpClient = HttpFactory.CreateClient(HttpClientNames.PwnedServerApi);
            _breach = await httpClient.GetFromJsonAsync<BreachDetails>(
                $"api/pwned/breach/{breachName}", BreachDetailJsonSerializerContext.DefaultTypeInfo);
        }

        private async Task Confirm() => await _modal.Confirm();

        private void OnDismissed(DismissalReason reason)
        {
            _breach = null!;
            _ = InvokeAsync(StateHasChanged);
        }
    }
}
