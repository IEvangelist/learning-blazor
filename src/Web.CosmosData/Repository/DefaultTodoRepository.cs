// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.CosmosData.Repository;

internal sealed class DefaultTodoRepository : ITodoRepository
{
    private readonly IRepository<Todo> _repository;

    public DefaultTodoRepository(IRepositoryFactory factory) =>
        _repository = factory.RepositoryOf<Todo>();

    /// <inheritdoc />
    ValueTask<Todo> ITodoRepository.CreateTodoAsync(
        Todo todo, CancellationToken cancellationToken) =>
        _repository.CreateAsync(todo, cancellationToken);

    /// <inheritdoc />
    ValueTask ITodoRepository.DeleteTodoAsync(
        string id, CancellationToken cancellationToken) =>
        _repository.DeleteAsync(id, cancellationToken: cancellationToken);

    /// <inheritdoc />
    ValueTask<IEnumerable<Todo>> ITodoRepository.ReadAllTodosAsync(
        Expression<Func<Todo, bool>> predicate, CancellationToken cancellationToken) =>
        _repository.GetAsync(predicate, cancellationToken);

    /// <inheritdoc />
    ValueTask<Todo> ITodoRepository.ReadTodoAsync(
        string id, CancellationToken cancellationToken) =>
        _repository.GetAsync(id, cancellationToken: cancellationToken);

    /// <inheritdoc />
    ValueTask<Todo> ITodoRepository.UpdateTodoAsync(
        Todo todo, CancellationToken cancellationToken) =>
         _repository.UpdateAsync(todo, cancellationToken);
}
