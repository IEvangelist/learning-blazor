// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.CosmosData.Repository;

public interface ITodoRepository
{
    /// <summary>
    /// Creates a new <paramref name="todo"/> instance, persisting
    /// it to the underlying Cosmos DB data store.
    /// </summary>
    /// <param name="todo">
    /// The <see cref="Todo"/> item to create.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional signal to trigger cancellation.
    /// </param>
    /// <returns>The created <see cref="Todo"/> item.</returns>
    ValueTask<Todo> CreateTodoAsync(
        Todo todo, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the <see cref="Todo"/> item for the given
    /// <paramref name="id"/>.
    /// </summary>
    /// <param name="id">
    /// The globally unique identifier (<see cref="Guid"/>) string
    /// that corresponds to the <see cref="Todo.Id"/> to read.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional signal to trigger cancellation.
    /// </param>
    /// <returns>
    /// The <see cref="Todo"/> item that corresponds to the given <paramref name="id"/>.
    /// </returns>
    ValueTask<Todo> ReadTodoAsync(
        string id, CancellationToken cancellationToken);

    /// <summary>
    /// Reads all <see cref="Todo"/> items from the
    /// underlying Cosmos DB data store that match
    /// the given <paramref name="predicate"/>.
    /// </summary>
    /// <param name="predicate">
    /// The expression used to match <see cref="Todo"/> items on.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional signal to trigger cancellation.
    /// </param>
    /// <returns>
    /// All <see cref="Todo"/> items matching the given <paramref name="predicate"/>.
    /// </returns>
    ValueTask<IEnumerable<Todo>> ReadAllTodosAsync(
        Expression<Func<Todo, bool>> predicate, CancellationToken cancellationToken);

    /// <summary>
    /// Updates the given <paramref name="todo"/> instance, persisting
    /// it to the underlying Cosmos DB data store.
    /// </summary>
    /// <param name="todo">
    /// The <see cref="Todo"/> item to update.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional signal to trigger cancellation.
    /// </param>
    /// <returns>The updated <see cref="Todo"/> item.</returns>
    ValueTask<Todo> UpdateTodoAsync(
        Todo todo, CancellationToken cancellationToken);

    /// <summary>
    /// Deletes the given <paramref name="todo"/> instance, removing
    /// it from the underlying Cosmos DB data store.
    /// </summary>
    /// <param name="id">
    /// The globally unique identifier (<see cref="Guid"/>) string
    /// that corresponds to the <see cref="Todo.Id"/> to delete.
    /// </param>
    /// <param name="cancellationToken">
    /// The optional signal to trigger cancellation.
    /// </param>
    /// <returns>
    /// The task operation that represents the asynchronous deletion.
    /// </returns>
    ValueTask DeleteTodoAsync(
        string id, CancellationToken cancellationToken);
}
