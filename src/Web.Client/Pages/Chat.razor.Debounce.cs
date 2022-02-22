// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Pages
{
    public sealed partial class Chat
    {
        private readonly HashSet<Actor> _usersTyping = new();
        private readonly SystemTimerAlias _debounceTimer = new()
        {
            Interval = 750,
            AutoReset = false
        };

        private bool _isTyping = false;

        public Chat() => _debounceTimer.Elapsed += OnDebounceElapsed;

        Task InitiateDebounceUserIsTypingAsync()
        {
            _debounceTimer.Stop();
            _debounceTimer.Start();

            return SetIsTypingAsync(true);
        }

        Task OnUserTypingAsync(Notification<ActorAction> actorAction) =>
            InvokeAsync(() =>
            {
                var (_, (user, isTyping)) = actorAction;
                _ = isTyping
                    ? _usersTyping.Add(new(user))
                    : _usersTyping.Remove(new(user));

                StateHasChanged();
            });

        Task SetIsTypingAsync(bool isTyping)
        {
            if (_isTyping && isTyping)
            {
                return Task.CompletedTask;
            }

            return HubConnection.ToggleUserTypingAsync(
                _isTyping = isTyping);
        }

        bool TryGetUsersTypingText(
            [NotNullWhen(true)] out string? text)
        {
            var users = _usersTyping
                ?.Select(a => a.UserName)
                ?.ToArray();

            text = users?.Length switch
            {
                0 or null => null,
                1 => $"💬 {Localizer["UserIsTypingFormat", users[0]]}",
                2 => $"💬 {Localizer["TwoUsersAreTypingFormat", users[0], users[1]]}",
                _ => $"💬 {Localizer["MultiplePeopleAreTyping"]}"
            };

            return text is not null;
        }

        async void OnDebounceElapsed(object? _, ElapsedEventArgs e) =>
            await SetIsTypingAsync(false);
    }
}
