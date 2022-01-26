// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public sealed partial class TaskCardComponent
    {
        private bool _isSaving = false;
        private bool _isDeleting = false;

        private bool _isWorking => _isSaving || _isDeleting;

        [Parameter, EditorRequired]
        public TodoItem Todo { get; set; } = null!;

        [Parameter, EditorRequired]
        public EventCallback<(TodoChangedDelegate CompleteCallback, TodoItem Todo)> TodoUpdated { get; set; }

        [Parameter, EditorRequired]
        public EventCallback<(TodoChangedDelegate CompleteCallback, TodoItem Todo)> TodoDeleted { get; set; }

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
            EventCallback<(TodoChangedDelegate CompleteCallback, TodoItem Todo)> callback)
        {
            if (callback.HasDelegate)
            {
                await callback.InvokeAsync((OnCallbackCompletedAsync, Todo));
            }
        }

        private Task OnCallbackCompletedAsync(TodoItem todo, TodoItemAction action) =>
            InvokeAsync(() =>
            {
                (_isSaving, _isDeleting) = action switch
                {
                    TodoItemAction.Created or
                    TodoItemAction.Updated => (false, _isDeleting),
                    TodoItemAction.Deleted => (_isSaving, false),
                    _ => (false, false)
                };

                StateHasChanged();
            });
    }
}
