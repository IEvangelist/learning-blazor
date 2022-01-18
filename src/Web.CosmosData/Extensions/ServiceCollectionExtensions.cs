// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

using Learning.Blazor.CosmosData.Repository;

namespace Learning.Blazor.CosmosData.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCosmosDataServices(
        this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddCosmosRepository(options =>
        {
            options.ContainerPerItemType = true;
            options.OptimizeBandwidth = true;
            options.AllowBulkExecution = true;

            options.DatabaseId = "learning-blazor-db";
            options.ContainerId = "todos";
            //options.CosmosConnectionString =
            //    configuration["RepositoryOptions__CosmosConnectionString"];
        })
        .AddTransient<ITodoRepository, DefaultTodoRepository>();
}
