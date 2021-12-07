// Copyright (c) 2021 David Pine. All rights reserved.
// Licensed under the MIT License.

namespace Learning.Blazor.Models.Requests;

public record ContactRequest(
    string FirstName,
    string LastName,
    string FromEmail,
    string Subject,
    string Body);
