// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Api.Controllers;

[
    Authorize,
    RequiredScope(new[] { "User.ApiAccess" }),
    ApiController,
    Route("api/todo")
]
public class TodoController : ControllerBase
{
    private readonly ITodoRepository _todoRepository;

    string? _emailAddress => User.GetFirstEmailAddress();

    public TodoController(ITodoRepository todoRepository) =>
        _todoRepository = todoRepository;

    [HttpGet]
    public async ValueTask<IEnumerable<TodoItem>> GetTodos(
        CancellationToken cancellationToken)
    {
        var emailAddress = _emailAddress;
        var todos =
            await _todoRepository.ReadAllTodosAsync(
                todo => todo.UserEmail == emailAddress,
                cancellationToken);

        return todos as IEnumerable<TodoItem> ?? Enumerable.Empty<TodoItem>();
    }

    [HttpGet("{id}")]
    public async ValueTask<TodoItem> GetTodoById(
        string id, CancellationToken cancellationToken) =>
        await _todoRepository.ReadTodoAsync(id, cancellationToken);

    [HttpPost]
    public async ValueTask<TodoItem> PostTodo(
        CancellationToken cancellationToken,
        [FromBody] CreateTodoRequest request) =>
        await _todoRepository.CreateTodoAsync(new Todo
        {
            Title = request.Title,
            UserEmail = request.UserEmail,
            IsCompleted = request.IsCompleted,
            DueDate = request.DueDate,
        }, cancellationToken);

    [HttpPut]
    public async ValueTask<TodoItem> PutTodo(
        [FromBody] TodoItem todo,
        CancellationToken cancellationToken) =>
        await _todoRepository.UpdateTodoAsync(todo, cancellationToken);

    [HttpDelete("{id}", Name = nameof(DeleteTodo))]
    public ValueTask DeleteTodo(string id, CancellationToken cancellationToken) =>
        _todoRepository.DeleteTodoAsync(id, cancellationToken);
}
