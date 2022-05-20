// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.PwnedApi.Services;

public interface IPwnedServices
{
    Task<BreachHeader[]?> GetBreachHeadersAsync(string email);

    Task<BreachDetails?> GetBreachDetailsAsync(string name);
}
