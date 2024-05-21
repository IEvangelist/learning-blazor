// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(["User.ApiAccess"]),
    ApiController,
    Route("api/todo")
]
public sealed class TodoController(ITodoRepository todoRepository) : ControllerBase
{
    string? _emailAddress => User.GetFirstEmailAddress();

    [HttpGet]
    public async ValueTask<IEnumerable<TodoItem>> GetTodos(
        CancellationToken cancellationToken)
    {
        var emailAddress = _emailAddress;
        var todos =
            await todoRepository.ReadAllTodosAsync(
                todo => todo.UserEmail == emailAddress,
                cancellationToken);

        var items = todos.Select(todo => (TodoItem)todo).ToList();
        return items;
    }

    [HttpGet("{id}")]
    public async ValueTask<TodoItem> GetTodoById(
        string id, CancellationToken cancellationToken)
    {
        var item = await todoRepository.ReadTodoAsync(id, cancellationToken);
        return (TodoItem)item;
    }

    [HttpPost]
    public async ValueTask<TodoItem> PostTodo(
        [FromBody] CreateTodoRequest request,
        CancellationToken cancellationToken)
    {
        var item = await todoRepository.CreateTodoAsync(new Todo
        {
            Title = request.Title,
            UserEmail = request.UserEmail,
            IsCompleted = request.IsCompleted,
            DueDate = request.DueDate,
        }, cancellationToken);

        return (TodoItem)item;
    }

    [HttpPut]
    public async ValueTask<TodoItem> PutTodo(
        [FromBody] TodoItem todoItem,
        CancellationToken cancellationToken)
    {
        var item = await todoRepository.UpdateTodoAsync((Todo)todoItem, cancellationToken);
        return (TodoItem)item;
    }

    [HttpDelete("{id}", Name = nameof(DeleteTodo))]
    public ValueTask DeleteTodo(string id, CancellationToken cancellationToken) =>
        todoRepository.DeleteTodoAsync(id, cancellationToken);
}
