// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Chat : IAsyncDisposable
    {
        private const string DefaultRoomName = "public";

        private readonly Stack<IDisposable> _subscriptions = new();

        [Parameter]
        public string? Room { get; set; } = DefaultRoomName;

        [Inject]
        public SharedHubConnection HubConnection { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            _subscriptions.Push(
                HubConnection.SubscribeToMessageReceived(OnMessageReceivedAsync));
            _subscriptions.Push(
                HubConnection.SubscribeToUserTyping(OnUserTypingAsync));

            await HubConnection.StartAsync();
            await HubConnection.JoinChatAsync(Room ?? DefaultRoomName);

            await _messageInput.FocusAsync();
        }

        async ValueTask IAsyncDisposable.DisposeAsync()
        {
            if (HubConnection is not null)
            {
                await HubConnection.LeaveChatAsync(Room ?? DefaultRoomName);
            }

            while (_subscriptions.TryPop(out var disposable))
            {
                disposable.Dispose();
            }
        }
    }
}
