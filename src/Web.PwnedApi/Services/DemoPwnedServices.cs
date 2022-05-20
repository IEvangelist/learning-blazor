// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi.Services;

public sealed class DemoPwnedServices : IPwnedServices
{
    Task<BreachDetails?> IPwnedServices.GetBreachDetailsAsync(string name) =>
        Task.FromResult<BreachDetails?>(null);

    Task<BreachHeader[]?> IPwnedServices.GetBreachHeadersAsync(string email) =>
        Task.FromResult<BreachHeader[]?>(null);
}
