// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Logging;

namespace Learning.Blazor.Pages
{
    public partial class Pwned
    {
        private EditContext? _editContext;
        private bool _isFormInvalid;
        private BreachHeader[] _breaches = Array.Empty<BreachHeader>();
        private ComponentState _state = ComponentState.Unknown;
        private PwnedComponenetModel _model = null!;
        private string? _filter = null!;

        private IEnumerable<BreachHeader> _filteredBreaches =>
            _filter is null
                ? _breaches
                : _breaches.Where(
                    breach => breach.Name.Contains(
                        _filter, StringComparison.OrdinalIgnoreCase));

        [Inject]
        public HttpClient Http { get; set; } = null!;

        protected override void OnInitialized()
        {
            if (_model == null)
            {
                _model = new PwnedComponenetModel();
            }

            _editContext = new(_model);
            _editContext.OnFieldChanged += OnModelChanged;
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
            StateHasChanged();
        }

        private void Reset()
        {
            _model.EmailAddress = null!;
            _breaches = Array.Empty<BreachHeader>();
            _state = ComponentState.Unknown;
        }

        private async ValueTask OnValidSubmitAsync(EditContext _)
        {
            try
            {
                _state = ComponentState.Loading;
                _breaches = (await Http.GetFromJsonAsync<BreachHeader[]>(
                    $"api/pwned/breaches/{_model.EmailAddress}", DefaultJsonSerialization.Options))!;

                _state = ComponentState.Loaded;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.Message);
                _state = ComponentState.Error;
            }
            finally
            {
                await InvokeAsync(StateHasChanged);
            }
        }
    }
}
