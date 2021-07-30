// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Timers;
using Learning.Blazor.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Learning.Blazor.Pages
{
    public sealed partial class Chat : IAsyncDisposable
    {
        private readonly Dictionary<string, ActorMessage> _messages = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<Actor> _usersTyping = new();
        private readonly Stack<IDisposable> _subscriptions = new();
        private readonly Timer _debouceTimer = new()
        {
            Interval = 750,
            AutoReset = false
        };

        private Guid? _messageId = null!;
        private string? _message = null!;
        private bool _isTyping = false;

        private ElementReference _messageInput;

        private const string DefaultRoomName = "public";

        [Parameter]
        public string? Room { get; set; } = DefaultRoomName;

        [Parameter]
        public ClaimsPrincipal User { get; set; } = null!;

        [Inject]
        public SingleHubConnection HubConnection { get; set; } = null!;

        public Chat() => _debouceTimer.Elapsed += OnDebouceElapsed;

        private async void OnDebouceElapsed(object sender, ElapsedEventArgs e) =>
            await SetIsTyping(false);

        private async Task OnKeyUp(KeyboardEventArgs args)
        {
            if (args is { Key: "Enter" } and { Code: "Enter" })
            {
                await SendMessage();
            }
        }

        async Task SendMessage()
        {
            if (_message is { Length: > 0 })
            {
                await HubConnection.PostOrUpdateMessageAsync(
                    Room ?? DefaultRoomName, _message, _messageId);

                _message = null;
                _messageId = null;

                StateHasChanged();
            }
        }

        async Task InitiateDebounceUserIsTyping()
        {
            _debouceTimer.Stop();
            _debouceTimer.Start();

            await SetIsTyping(true);
        }

        async Task SetIsTyping(bool isTyping)
        {
            if (_isTyping && isTyping)
            {
                return;
            }

            await HubConnection.ToggleUserIsTypingAsync(_isTyping = isTyping);
        }

        async Task AppendToMessage(string text)
        {
            _message += text;

            await _messageInput.FocusAsync();
            await SetIsTyping(false);
        }

        bool OwnsMessage(string user) => User?.Identity?.Name == user;

        async Task StartEdit(ActorMessage message)
        {
            if (!OwnsMessage(message.UserName))
            {
                return;
            }

            await InvokeAsync(
                async () =>
                {
                    _messageId = message.Id;
                    _message = message.Text;

                    await _messageInput.FocusAsync();

                    StateHasChanged();
                });
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.LeaveTweetsAsync();
            }

            while (_subscriptions.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
