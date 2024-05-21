// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages;

public sealed partial class Chat
{
    private readonly Dictionary<Guid, ActorMessage> _messages = [];

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

                StateHasChanged();

                await JavaScript.ScrollIntoViewAsync(
                    $"[id='{message.Payload.Id}']");
            });

    Task OnKeyUpAsync(KeyboardEventArgs args)
    {
        if (_isSending)
        {
            return Task.CompletedTask;
        }

        return args switch
        {
            { Key: "Enter" } => SendMessageAsync(),
            _ => Task.CompletedTask
        };
    }

    async Task SendMessageAsync()
    {
        if (_isSending || string.IsNullOrWhiteSpace(_message))
        {
            return;
        }

        try
        {
            _isSending = true;

            await HubConnection.PostOrUpdateMessageAsync(
                Room ?? DefaultRoomName, _message, _messageId);

            _message = null;
            _messageId = null;
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
