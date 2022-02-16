// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Chat
    {
        private readonly Dictionary<Guid, ActorMessage> _messages = new();

        private Guid? _messageId = null!;
        private string? _message = null!;
        private bool _isSending = false;
        private ElementReference _messageInput;

        bool OwnsMessage(string user) => User?.Identity?.Name == user;

        Task OnMessageReceivedAsync(Notification<ActorMessage> message) =>
            InvokeAsync(
                async () =>
                {
                    _messages[message.Payload.Id] = message;

                    await JavaScript.ScrollIntoViewAsync(
                        $"#{message.Payload.Id}");

                    StateHasChanged();
                });

        Task OnKeyUpAsync(KeyboardEventArgs args) => (_isSending, args) switch
        {
            (false, { Key: "Enter" } and { Code: "Enter" }) => SendMessageAsync(),
            _ => Task.CompletedTask
        };

        async Task SendMessageAsync()
        {
            _isSending = true;

            try
            {
                if (_message is { Length: > 0 })
                {
                    await HubConnection.PostOrUpdateMessageAsync(
                        Room ?? DefaultRoomName, _message, _messageId);

                    _message = null;
                    _messageId = null;
                }
            }
            finally
            {
                _isSending = false;
            }
        }

        async Task OnEditMessageAsync(ActorMessage message)
        {
            if (!OwnsMessage(message.UserName))
            {
                return;
            }

            _messageId = message.Id;
            _message = message.Text;

            await _messageInput.FocusAsync();
        }
    }
}
