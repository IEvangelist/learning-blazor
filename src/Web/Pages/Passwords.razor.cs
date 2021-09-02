// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.ComponentModels;
using Learning.Blazor.Extensions;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using System.Net.Http.Json;
using HaveIBeenPwned.Client.Models;

namespace Learning.Blazor.Pages;

public partial class Passwords
{
    private readonly PasswordsComponentModel _model = new();

    private EditContext? _editContext;
    private InputText _passwordInput = null!;
    private bool _isFormInvalid;
    private PwnedPassword? _pwnedPassword = null!;
    private ComponentState _state = ComponentState.Unknown;

    [Inject]
    public HttpClient Http { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        _editContext = new(_model);
        _editContext.OnFieldChanged += OnModelChanged;

        await (_passwordInput?.Element?.FocusAsync(preventScroll: true) ?? ValueTask.CompletedTask);
    }

    private void OnModelChanged(object? sender, FieldChangedEventArgs e)
    {
        _isFormInvalid = !_editContext?.Validate() ?? true;
        StateHasChanged();
    }

    private void Reset()
    {
        _model.PlainTextPassword = null!;
        _pwnedPassword = null!;
        _state = ComponentState.Unknown;
        _editContext?.MarkAsUnmodified();
    }

    private async ValueTask OnValidSubmitAsync(EditContext _)
    {
        try
        {
            _state = ComponentState.Loading;
            _pwnedPassword = (await Http.GetFromJsonAsync<PwnedPassword>(
                $"api/pwned/passwords/{_model.PlainTextPassword}", DefaultJsonSerialization.Options))!;

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
