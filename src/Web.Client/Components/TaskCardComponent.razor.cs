// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class TaskCardComponent
    {
        bool _isSaving = false;
        bool _isDeleting = false;

        bool _isWorking => _isSaving || _isDeleting;

        [Parameter, EditorRequired]
        public TodoItem Todo { get; set; } = null!;

        [Parameter, EditorRequired]
        public EventCallback<(Func<Task> CompleteCallback, TodoItem Todo)> TodoUpdated { get; set; }

        [Parameter, EditorRequired]
        public EventCallback<(Func<Task> CompleteCallback, TodoItem Todo)> TodoDeleted { get; set; }

        private Task OnClickAsync(bool isDelete)
        {
            if (isDelete)
            {
                _isDeleting = true;
                return OnHandleAsync(TodoDeleted);
            }
            else
            {
                _isSaving = true;
                return OnHandleAsync(TodoUpdated);
            }            
        }

        private async Task OnHandleAsync(
            EventCallback<(Func<Task> CompleteCallback, TodoItem Todo)> callback)
        {
            if (callback.HasDelegate)
            {
                await callback.InvokeAsync((OnCallbackCompletedAsync, Todo));
            }
        }

        private Task OnCallbackCompletedAsync() =>
            InvokeAsync(() =>
            {
                (_isSaving, _isDeleting) = (false, false);
                StateHasChanged();
            });
    }
}
