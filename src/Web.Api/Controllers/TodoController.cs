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

    public TodoController(ITodoRepository todoRepository) =>
        _todoRepository = todoRepository;

    [HttpGet(Name = nameof(GetTodos))]
    public async ValueTask<IEnumerable<TodoItem>> GetTodos(
        CancellationToken cancellationToken)
    {
        var todos =
            await _todoRepository.ReadAllTodosAsync(
                todo => todo.UserEmail == User.GetFirstEmailAddress(),
                cancellationToken);

        return todos as IEnumerable<TodoItem> ?? Enumerable.Empty<TodoItem>();
    }

    [HttpGet("{id}", Name = nameof(GetTodoById))]
    public async ValueTask<TodoItem> GetTodoById(
        string id, CancellationToken cancellationToken) =>
        await _todoRepository.ReadTodoAsync(id, cancellationToken);

    [HttpPost(Name = nameof(PostTodo))]
    public async ValueTask<TodoItem> PostTodo(
        CancellationToken cancellationToken,
        [FromBody] TodoItem todo) =>
        await _todoRepository.CreateTodoAsync(todo, cancellationToken);

    [HttpPut(Name = nameof(PutTodo))]
    public async ValueTask<TodoItem> PutTodo(
        [FromBody] TodoItem todo,
        CancellationToken cancellationToken) =>
        await _todoRepository.UpdateTodoAsync(todo, cancellationToken);

    [HttpDelete("{id}", Name = nameof(DeleteTodo))]
    public ValueTask DeleteTodo(string id, CancellationToken cancellationToken) =>
        _todoRepository.DeleteTodoAsync(id, cancellationToken);
}
