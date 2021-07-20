// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Learning.Blazor.ComponentModels;
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
        private ComponentState _state = ComponentState.Loaded;

        [Inject]
        public HttpClient Http { get; set; } = null!;

        [Parameter]
        public PwnedComponenetModel Model { get; set; } = null!;

        protected override void OnInitialized()
        {
            if (Model == null)
            {
                Model = new PwnedComponenetModel();
            }

            _editContext = new(Model);
            _editContext.OnFieldChanged += OnModelChanged;
        }

        private void OnModelChanged(object? sender, FieldChangedEventArgs e)
        {
            _isFormInvalid = !_editContext?.Validate() ?? true;
            StateHasChanged();
        }

        private async ValueTask OnValidSubmitAsync(EditContext context)
        {
            try
            {
                _state = ComponentState.Loading;
                //if (context?.Validate() ?? false)
                {
                    var email = Model.EmailAddress;
                    _breaches = (await Http.GetFromJsonAsync<BreachHeader[]>($"api/breaches/{email}"))!;
                }
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
