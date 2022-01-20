// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Components
{
    public partial class TaskListComponent
    {
        private readonly HashSet<TodoItem> _todos = new();

        private bool _show = false;
        private CreateTodoRequest? _newTodo = null;
        private bool _isAddingTodo = false;
        private bool _isSaving = false;
        private string _showClass => _show ? "is-active" : "";

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; } = null!;

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            var client = HttpFactory.CreateClient(HttpClientNames.ServerApi);
            var todos = await client.GetFromJsonAsync<IEnumerable<TodoItem>>("api/todo");
            foreach (var todo in todos ?? Enumerable.Empty<TodoItem>())
            {
                _ = _todos.Add(todo);
            }
        }

        private void Show() => _show = true;

        private void Dismiss() => _show = false;

        async Task OnTodoUpdatedAsync((Func<Task> CompleteCallback, TodoItem Todo) pair)
        {
            var (completionCallback, todo) = pair;
            try
            {
                var client = HttpFactory.CreateClient(HttpClientNames.ServerApi);
                using var response = await client.PutAsJsonAsync(
                    "api/todo", todo);

                response.EnsureSuccessStatusCode();
            }
            finally
            {
                _show = _todos.Any();
                await completionCallback();
            }
        }

        async Task OnTodoDeletedAsync((Func<Task> CompleteCallback, TodoItem Todo) pair)
        {
            var (completionCallback, todo) = pair;
            try
            {
                var client = HttpFactory.CreateClient(HttpClientNames.ServerApi);
                using var response = await client.DeleteAsync(
                    $"api/todo/{todo.Id}");

                response.EnsureSuccessStatusCode();

                _ = _todos.Remove(todo);
            }
            finally
            {
                _show = _todos.Any();
                await completionCallback();
            }
        }

        async Task OnTodoSavedAsync(CreateTodoRequest todo)
        {
            _isSaving = true;
            try
            {
                var client = HttpFactory.CreateClient(HttpClientNames.ServerApi);
                using var response = await client.PostAsJsonAsync(
                    "api/todo", todo);

                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                if (json is not null && json.FromJson<TodoItem>() is TodoItem todoItem)
                {
                    _ = _todos.Add(todoItem);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message);
            }
            finally
            {
                _isSaving = false;
                _show = _todos.Any();
                (_newTodo, _isAddingTodo) = (null, false);

                await InvokeAsync(StateHasChanged);
            }
        }

        void StartAddingTodo() =>
            (_newTodo, _isAddingTodo) =
                (new CreateTodoRequest { UserEmail = User.GetFirstEmailAddress()! }, true);

        void CancelNewTodo() =>
            (_newTodo, _isAddingTodo) = (null, false);
    }
}
